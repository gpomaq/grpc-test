using System.Collections.Concurrent;

namespace GrpcTest.Services
{
    // Bolivia does not observe daylight saving time, so a fixed UTC-4 offset always holds.
    // Centralizing "now" here keeps every timestamp in the mock expressed in local Bolivia
    // time instead of UTC, matching how the real backend reports dates to clients.
    public static class BoliviaClock
    {
        public static readonly TimeSpan Offset = TimeSpan.FromHours(-4);

        public static DateTimeOffset Now => DateTimeOffset.UtcNow.ToOffset(Offset);
    }

    // Progreso de un tema para un usuario dentro de un departamento concreto. El mismo
    // tema puede estar completado en La Paz y sin empezar en Santa Cruz.
    public class TopicProgress
    {
        public string DepartmentCode { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;

        // "not_started" | "in_progress" | "completed"
        public string Status { get; set; } = TriviaMockDatabase.TopicNotStarted;
        public int BestScore { get; set; }
        public int TimesCompleted { get; set; }

        // Momento (hora Bolivia) de la última vez que se reclamaron recompensas de este tema.
        // Status es histórico y nunca vuelve atrás, así que no alcanza para responder
        // "¿el usuario hizo el desafío de hoy?": para eso se necesita la fecha.
        public DateTimeOffset? LastCompletedAt { get; set; }
    }

    public class UserState
    {
        public string IdUserProfile { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = "Usuario Demo";
        public string Gender { get; set; } = "female";
        public int Age { get; set; } = 28;
        public string AccountOpeningBranch { get; set; } = "LP";
        public int WeeklyAttemptsLeft { get; set; } = 3;
        public DateTimeOffset NextAttemptsResetAt { get; set; }
        public int TotalXp { get; set; } = 150;
        public int TotalYastaCoins { get; set; } = 30;

        // Tracks progress for each department
        public ConcurrentDictionary<string, DepartmentProgressPb> Departments { get; set; } = new();

        // Progreso por tema, con clave "<DEPT>|<topic_id>".
        public ConcurrentDictionary<string, TopicProgress> Topics { get; set; } = new();
    }

    public class TriviaSessionState
    {
        public string SessionId { get; set; } = string.Empty;
        public string IdUserProfile { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; } = 0;
        public int CorrectAnswersCount { get; set; } = 0;
        public string LastQuestionId { get; set; } = string.Empty;
        public DateTimeOffset LastAccessedAt { get; set; } = BoliviaClock.Now;

        // Muestra aleatoria de preguntas del banco del tema, fijada al iniciar la sesión:
        // repetir un tema no repite el mismo set ni el mismo orden.
        public List<string> QuestionOrder { get; set; } = new();

        // Guards read-modify-write of this session's mutable counters (e.g. concurrent
        // SubmitAnswer retries) so a double request can't advance/score a question twice.
        public readonly object Lock = new();

        public void Touch() => LastAccessedAt = BoliviaClock.Now;
    }

    public static class TriviaMockDatabase
    {
        private static readonly ConcurrentDictionary<string, UserState> Users = new();
        private static readonly ConcurrentDictionary<string, TriviaSessionState> Sessions = new();

        // Sesión activa por usuario+departamento+tema, con clave "<user>|<DEPT>|<topic_id>".
        private static readonly ConcurrentDictionary<string, string> TopicSessions = new();

        public static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(30);

        public const string TopicNotStarted = "not_started";
        public const string TopicInProgress = "in_progress";
        public const string TopicCompleted = "completed";

        public const string DeptLocked = "locked";
        public const string DeptUnlocked = "unlocked";
        public const string DeptInProgress = "in_progress";
        public const string DeptCompleted = "completed";

        static TriviaMockDatabase()
        {
            var timer = new System.Threading.Timer(state =>
            {
                try
                {
                    var now = BoliviaClock.Now;
                    var expired = Sessions.Where(kv => (now - kv.Value.LastAccessedAt) > SessionTimeout)
                                          .Select(kv => kv.Key)
                                          .ToList();
                    foreach (var id in expired)
                    {
                        Sessions.TryRemove(id, out var _);
                        var topicEntries = TopicSessions.Where(d => d.Value == id).Select(d => d.Key).ToList();
                        foreach (var topicKey in topicEntries)
                        {
                            TopicSessions.TryRemove(topicKey, out var _);
                        }
                    }
                }
                catch { }
            }, null, SessionTimeout, SessionTimeout);
        }

        // Maximum trivia attempts a user gets per week before NextAttemptsResetAt.
        public const int WeeklyAttemptsMax = 3;

        // Computes the next Monday 00:00:00 in Bolivia local time — used both to seed a new
        // user's quota window and to recompute it once the current window has elapsed.
        public static DateTimeOffset ComputeNextWeeklyResetAt()
        {
            var now = BoliviaClock.Now;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;
            if (daysUntilMonday == 0) daysUntilMonday = 7;
            var nextMonday = now.Date.AddDays(daysUntilMonday);
            return new DateTimeOffset(nextMonday, BoliviaClock.Offset);
        }

        // Lazily resets the weekly quota once its window has elapsed. There's no persistence
        // or cron here, so the reset is computed on read/write access instead — the same
        // "check on access" pattern a real backend would use for a per-user counter.
        public static void EnsureWeeklyAttemptsFresh(UserState user)
        {
            if (BoliviaClock.Now >= user.NextAttemptsResetAt)
            {
                user.WeeklyAttemptsLeft = WeeklyAttemptsMax;
                user.NextAttemptsResetAt = ComputeNextWeeklyResetAt();
            }
        }

        // Only these two identities exist in the mock "database" — mirrors a real backend
        // where id_user_profile must resolve to an actual registered account.
        private sealed record UserSeed(string DocumentNumber, string Name, string Gender, int Age);

        private static readonly Dictionary<string, UserSeed> ValidUsers = new()
        {
            { "1", new UserSeed("123456", "John", "male", 28) },
            { "2", new UserSeed("654321", "Fiora", "female", 34) }
        };

        public static UserState? GetOrCreateUser(string idUserProfile)
        {
            if (!ValidUsers.TryGetValue(idUserProfile, out var seed))
            {
                return null;
            }

            return Users.GetOrAdd(idUserProfile, id =>
            {
                var state = new UserState
                {
                    IdUserProfile = id,
                    DocumentNumber = seed.DocumentNumber,
                    Name = seed.Name,
                    Gender = seed.Gender,
                    Age = seed.Age,
                    AccountOpeningBranch = "LP",
                    WeeklyAttemptsLeft = WeeklyAttemptsMax,
                    NextAttemptsResetAt = ComputeNextWeeklyResetAt(),
                    TotalXp = 250,
                    TotalYastaCoins = 50
                };

                InitializeUserDepartments(state);
                return state;
            });
        }

        public static void InitializeUserDepartments(UserState user)
        {
            int totalTopics = TriviaCatalog.TotalTopicsCount;

            foreach (var kv in TriviaCatalog.DepartmentNames)
            {
                bool isOpening = string.Equals(kv.Key, user.AccountOpeningBranch, StringComparison.OrdinalIgnoreCase);
                user.Departments[kv.Key] = new DepartmentProgressPb
                {
                    Code = kv.Key,
                    Name = kv.Value,
                    Status = isOpening ? DeptUnlocked : DeptLocked,
                    CompletedQuestions = 0,
                    TotalQuestions = totalTopics * TriviaCatalog.QuestionsPerSession,
                    CompletedTopics = 0,
                    TotalTopics = totalTopics
                };
            }
        }

        public static void UnlockOtherDepartments(UserState user)
        {
            foreach (var kv in user.Departments)
            {
                if (kv.Value.Status == DeptLocked)
                {
                    kv.Value.Status = DeptUnlocked;
                }
            }
        }

        // ---- Progreso por tema ----

        private static string TopicKey(string departmentCode, string topicId) => $"{departmentCode.ToUpper()}|{topicId}";

        public static TopicProgress GetOrCreateTopicProgress(UserState user, string departmentCode, TriviaTopic topic)
        {
            return user.Topics.GetOrAdd(TopicKey(departmentCode, topic.Id), _ => new TopicProgress
            {
                DepartmentCode = departmentCode.ToUpper(),
                CategoryCode = topic.CategoryCode,
                TopicId = topic.Id
            });
        }

        public static TopicProgress? FindTopicProgress(UserState user, string departmentCode, string topicId)
        {
            return user.Topics.TryGetValue(TopicKey(departmentCode, topicId), out var progress) ? progress : null;
        }

        // El desafío diario se evalúa contra el día en curso (hora Bolivia), igual que la
        // rotación del tema: haber completado este mismo tema en otra fecha no cuenta como
        // haber cumplido el reto de hoy.
        public static bool WasCompletedToday(TopicProgress? progress)
        {
            return progress?.LastCompletedAt is DateTimeOffset completedAt &&
                   completedAt.Date == BoliviaClock.Now.Date;
        }

        public static int CountCompletedTopics(UserState user, string departmentCode, string? categoryCode = null)
        {
            string dept = departmentCode.ToUpper();
            return user.Topics.Values.Count(p =>
                p.Status == TopicCompleted &&
                string.Equals(p.DepartmentCode, dept, StringComparison.OrdinalIgnoreCase) &&
                (categoryCode == null || string.Equals(p.CategoryCode, categoryCode, StringComparison.OrdinalIgnoreCase)));
        }

        // Un departamento se considera completado cuando el usuario terminó al menos un tema
        // de cada categoría en ese departamento. Exigir los 16 temas haría imposible ver el
        // desbloqueo en un mock con 3 intentos semanales, y exigir uno solo lo volvería trivial.
        public static bool IsDepartmentCompleted(UserState user, string departmentCode)
        {
            return TriviaCatalog.Categories.All(category => CountCompletedTopics(user, departmentCode, category.Code) > 0);
        }

        // Recalcula los contadores del departamento a partir del progreso por tema. El estado
        // solo avanza (unlocked -> in_progress -> completed): nunca se revierte un desbloqueo.
        public static void RefreshDepartmentProgress(UserState user, string departmentCode)
        {
            if (!user.Departments.TryGetValue(departmentCode.ToUpper(), out var dept))
            {
                return;
            }

            int completedTopics = CountCompletedTopics(user, departmentCode);
            dept.CompletedTopics = completedTopics;
            dept.CompletedQuestions = completedTopics * TriviaCatalog.QuestionsPerSession;

            if (IsDepartmentCompleted(user, departmentCode))
            {
                dept.Status = DeptCompleted;
            }
            else if (dept.Status == DeptUnlocked || dept.Status == DeptCompleted)
            {
                bool hasActivity = user.Topics.Values.Any(p =>
                    string.Equals(p.DepartmentCode, departmentCode.ToUpper(), StringComparison.OrdinalIgnoreCase) &&
                    p.Status != TopicNotStarted);

                dept.Status = hasActivity ? DeptInProgress : DeptUnlocked;
            }
        }

        // ---- Sesiones ----

        public static TriviaSessionState? GetSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId)) return null;
            return Sessions.TryGetValue(sessionId, out var session) ? session : null;
        }

        private static string SessionKey(string idUserProfile, string departmentCode, string topicId)
            => $"{idUserProfile}|{TopicKey(departmentCode, topicId)}";

        public static TriviaSessionState? FindSessionForTopic(string idUserProfile, string departmentCode, string topicId)
        {
            return TopicSessions.TryGetValue(SessionKey(idUserProfile, departmentCode, topicId), out string? sessionId)
                ? GetSession(sessionId)
                : null;
        }

        // Sesión reanudable: existe y todavía le quedan preguntas por responder.
        public static TriviaSessionState? GetActiveSessionForTopic(string idUserProfile, string departmentCode, string topicId)
        {
            var session = FindSessionForTopic(idUserProfile, departmentCode, topicId);
            return session != null && session.CurrentQuestionIndex < session.QuestionOrder.Count ? session : null;
        }

        // Sesión abierta y vigente, incluida la que ya respondió todas las preguntas pero
        // aún no reclamó recompensas: para la card del reto diario eso sigue siendo
        // "empezado", no "sin empezar".
        public static bool HasOpenSessionForTopic(string idUserProfile, string departmentCode, string topicId)
        {
            var session = FindSessionForTopic(idUserProfile, departmentCode, topicId);
            return session != null && (BoliviaClock.Now - session.LastAccessedAt) <= SessionTimeout;
        }

        // Fisher-Yates sobre el banco del tema; se toman las primeras QuestionsPerSession.
        // Así cada sesión juega un subconjunto distinto de un banco más grande.
        private static List<string> SampleQuestionIds(TriviaTopic topic)
        {
            var ids = topic.Questions.Select(q => q.Id).ToList();
            for (int i = ids.Count - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                (ids[i], ids[j]) = (ids[j], ids[i]);
            }
            return ids.Take(Math.Min(TriviaCatalog.QuestionsPerSession, ids.Count)).ToList();
        }

        public static TriviaSessionState StartNewSession(string idUserProfile, string departmentCode, TriviaTopic topic)
        {
            string sessionId = Guid.NewGuid().ToString();
            var session = new TriviaSessionState
            {
                SessionId = sessionId,
                IdUserProfile = idUserProfile,
                DepartmentCode = departmentCode.ToUpper(),
                CategoryCode = topic.CategoryCode,
                TopicId = topic.Id,
                CurrentQuestionIndex = 0,
                CorrectAnswersCount = 0,
                QuestionOrder = SampleQuestionIds(topic)
            };

            Sessions[sessionId] = session;
            TopicSessions[SessionKey(idUserProfile, departmentCode, topic.Id)] = sessionId;

            return session;
        }

        public static void TerminateSession(string sessionId)
        {
            if (Sessions.TryRemove(sessionId, out var session))
            {
                TopicSessions.TryRemove(SessionKey(session.IdUserProfile, session.DepartmentCode, session.TopicId), out _);
            }
        }

        // Resolves the question at a given position within this session's own sampled order,
        // instead of the topic bank order.
        public static MockQuestion? GetQuestionForSession(TriviaSessionState session, int index)
        {
            if (index < 0 || index >= session.QuestionOrder.Count)
                return null;

            var topic = TriviaCatalog.GetTopic(session.TopicId);
            if (topic == null)
                return null;

            return TriviaCatalog.GetQuestion(topic, session.QuestionOrder[index]);
        }

        public static int GetSessionQuestionsCount(TriviaSessionState session) => session.QuestionOrder.Count;
    }
}
