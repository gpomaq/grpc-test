using Grpc.Core;
using GrpcTest.Errors;

namespace GrpcTest.Services
{
    public class TriviaService : GrpcTest.TriviaService.TriviaServiceBase
    {
        // Only validates presence of the token and of the id_user_profile claim.
        // Format/signature/expiration are the gateway's responsibility, not ours.
        private (string idUserProfile, string documentNumber, string errorCode, string errorMsg) ParseJwtFromMetadata(Metadata metadata)
        {
            var authHeader = metadata.FirstOrDefault(m => string.Equals(m.Key, "authorization", StringComparison.OrdinalIgnoreCase));
            if (authHeader == null || string.IsNullOrEmpty(authHeader.Value))
            {
                return (string.Empty, string.Empty, ErrorCode.ERR009, ErrorMessage.ERR009);
            }

            var val = authHeader.Value;
            var token = val.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? val.Substring(7).Trim() : val.Trim();

            try
            {
                var parts = token.Split('.');
                if (parts.Length < 2)
                {
                    return (string.Empty, string.Empty, ErrorCode.ERR011, ErrorMessage.ERR011);
                }

                var payload = parts[1];
                payload = payload.Replace('-', '+').Replace('_', '/');
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var jsonStr = System.Text.Encoding.UTF8.GetString(bytes);

                using var doc = System.Text.Json.JsonDocument.Parse(jsonStr);
                var root = doc.RootElement;

                string idUserProfile = string.Empty;
                if (root.TryGetProperty("id_user_profile", out var idProp))
                {
                    idUserProfile = idProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("idUserProfile", out var idProp2))
                {
                    idUserProfile = idProp2.GetString() ?? string.Empty;
                }

                string documentNumber = string.Empty;
                if (root.TryGetProperty("document_number", out var docProp))
                {
                    documentNumber = docProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("documentNumber", out var docProp2))
                {
                    documentNumber = docProp2.GetString() ?? string.Empty;
                }

                if (string.IsNullOrEmpty(idUserProfile))
                {
                    return (string.Empty, string.Empty, ErrorCode.ERR011, ErrorMessage.ERR011);
                }

                return (idUserProfile, documentNumber, string.Empty, string.Empty);
            }
            catch (Exception)
            {
                return (string.Empty, string.Empty, ErrorCode.ERR011, ErrorMessage.ERR011);
            }
        }

        // Resolución común a casi todos los métodos: token -> usuario -> departamento habilitado.
        // Si el department_code viene vacío se asume la sucursal de apertura del usuario.
        private (UserState? user, string departmentCode, string errorCode, string errorMsg) ResolveUserAndDepartment(
            Metadata metadata, string requestedDepartmentCode, bool requireUnlocked = true)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(metadata);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return (null, string.Empty, errorCode, errorMsg);
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return (null, string.Empty, ErrorCode.ERR010, ErrorMessage.ERR010);
            }

            TriviaMockDatabase.EnsureWeeklyAttemptsFresh(user);

            string deptCode = string.IsNullOrWhiteSpace(requestedDepartmentCode)
                ? user.AccountOpeningBranch
                : requestedDepartmentCode.Trim().ToUpper();

            if (!user.Departments.TryGetValue(deptCode, out var deptProgress))
            {
                return (null, string.Empty, ErrorCode.ERR013, ErrorMessage.ERR013);
            }

            if (requireUnlocked && deptProgress.Status == TriviaMockDatabase.DeptLocked)
            {
                return (null, string.Empty, ErrorCode.ERR007, ErrorMessage.ERR007);
            }

            return (user, deptCode, string.Empty, string.Empty);
        }

        // Estado agregado de una categoría dentro de un departamento, derivado del progreso
        // por tema del usuario.
        private static (string status, int completedTopics) BuildCategoryStatus(UserState user, string departmentCode, TriviaCategory category)
        {
            int completed = TriviaMockDatabase.CountCompletedTopics(user, departmentCode, category.Code);
            if (completed >= category.Topics.Count)
            {
                return (TriviaMockDatabase.TopicCompleted, completed);
            }

            bool started = category.Topics.Any(t =>
            {
                var progress = TriviaMockDatabase.FindTopicProgress(user, departmentCode, t.Id);
                return progress != null && progress.Status != TriviaMockDatabase.TopicNotStarted;
            });

            return (started ? TriviaMockDatabase.TopicInProgress : TriviaMockDatabase.TopicNotStarted, completed);
        }

        // Estado del desafío diario referido a HOY, no al histórico del tema: completar este
        // mismo tema hace tres semanas dejaría el status en "completed" para siempre y la app
        // pintaría "ya hiciste el reto de hoy" sin que el usuario lo haya tocado.
        private static string BuildDailyChallengeStatus(UserState user, string departmentCode, TriviaTopic topic)
        {
            var progress = TriviaMockDatabase.FindTopicProgress(user, departmentCode, topic.Id);
            if (TriviaMockDatabase.WasCompletedToday(progress))
            {
                return TriviaMockDatabase.TopicCompleted;
            }

            if (TriviaMockDatabase.HasOpenSessionForTopic(user.IdUserProfile, departmentCode, topic.Id))
            {
                return TriviaMockDatabase.TopicInProgress;
            }

            return TriviaMockDatabase.TopicNotStarted;
        }

        public override Task<GetProfileBaseResponsePb> GetProfile(GetProfileRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new GetProfileBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetProfileBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = ErrorMessage.ERR010
                });
            }

            TriviaMockDatabase.EnsureWeeklyAttemptsFresh(user);
            string nextResetDate = user.NextAttemptsResetAt.ToString("dd/MM/yyyy");

            var response = new GetProfileResponsePb
            {
                User = new UserProfilePb
                {
                    Id = user.IdUserProfile,
                    Name = user.Name,
                    Gender = user.Gender,
                    Age = user.Age,
                    AccountOpeningBranch = user.AccountOpeningBranch
                },
                Quota = new UserQuotaPb
                {
                    WeeklyAttemptsMax = TriviaMockDatabase.WeeklyAttemptsMax,
                    WeeklyAttemptsLeft = user.WeeklyAttemptsLeft,
                    NextResetDate = nextResetDate
                },
                GlobalProgress = new GlobalProgressPb
                {
                    TotalXp = user.TotalXp,
                    TotalYastaCoins = user.TotalYastaCoins
                }
            };

            response.GlobalProgress.Departments.AddRange(user.Departments.Values.OrderBy(d => d.Name));

            var baseResponse = new GetProfileBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000
            };

            return Task.FromResult(baseResponse);
        }

        // Pantalla 1: a partir del departamento, lista las categorías disponibles y el
        // tema destacado del día.
        public override Task<GetCategoriesBaseResponsePb> GetCategories(GetCategoriesRequestPb request, ServerCallContext context)
        {
            var (user, deptCode, errorCode, errorMsg) = ResolveUserAndDepartment(context.RequestHeaders, request.DepartmentCode);
            if (user == null)
            {
                return Task.FromResult(new GetCategoriesBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var deptProgress = user.Departments[deptCode];

            var response = new GetCategoriesResponsePb
            {
                DepartmentCode = deptCode,
                DepartmentName = deptProgress.Name,
                DepartmentStatus = deptProgress.Status
            };

            foreach (var category in TriviaCatalog.Categories)
            {
                var (status, completedTopics) = BuildCategoryStatus(user, deptCode, category);
                response.Categories.Add(new CategoryPb
                {
                    Code = category.Code,
                    Label = category.Label,
                    IconKey = category.IconKey,
                    Description = category.Description,
                    TotalTopics = category.Topics.Count,
                    CompletedTopics = completedTopics,
                    Status = status
                });
            }

            // El desafío diario apunta a un tema concreto: el cliente va directo a
            // GetTopicResource(topic_id) y luego a la trivia, sin pasar por GetTopics.
            var daily = TriviaCatalog.GetDailyChallengeTopic(BoliviaClock.Now);
            var dailyProgress = TriviaMockDatabase.FindTopicProgress(user, deptCode, daily.Id);

            response.DailyChallenge = new DailyChallengePb
            {
                Date = BoliviaClock.Now.ToString("dd/MM/yyyy"),
                TopicId = daily.Id,
                TopicTitle = daily.Title,
                CategoryCode = daily.CategoryCode,
                CategoryLabel = daily.CategoryLabel,
                IconKey = daily.IconKey,
                Title = $"Desafío diario: {daily.Title}",
                Description = $"Hoy te retamos con {daily.Title} ({daily.CategoryLabel}). Revisa el recurso y demuestra lo que sabes.",
                TotalQuestions = Math.Min(TriviaCatalog.QuestionsPerSession, daily.Questions.Count),
                Status = BuildDailyChallengeStatus(user, deptCode, daily),
                BestScore = dailyProgress?.BestScore ?? 0,
                TimesCompleted = dailyProgress?.TimesCompleted ?? 0,
                ResourceType = daily.Resource.Type
            };

            return Task.FromResult(new GetCategoriesBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000
            });
        }

        // Pantalla 2: temas de la categoría seleccionada. El desafío diario ya no pasa por
        // aquí (va directo al recurso del tema); solo se marca el tema destacado del día.
        public override Task<GetTopicsBaseResponsePb> GetTopics(GetTopicsRequestPb request, ServerCallContext context)
        {
            var (user, deptCode, errorCode, errorMsg) = ResolveUserAndDepartment(context.RequestHeaders, request.DepartmentCode);
            if (user == null)
            {
                return Task.FromResult(new GetTopicsBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var category = TriviaCatalog.GetCategory(request.CategoryCode);
            if (category == null)
            {
                return Task.FromResult(new GetTopicsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR016,
                    Message = ErrorMessage.ERR016
                });
            }

            var dailyTopic = TriviaCatalog.GetDailyChallengeTopic(BoliviaClock.Now);

            var response = new GetTopicsResponsePb
            {
                DepartmentCode = deptCode,
                CategoryCode = category.Code,
                CategoryLabel = category.Label,
                IconKey = category.IconKey
            };

            foreach (var topic in category.Topics)
            {
                var progress = TriviaMockDatabase.FindTopicProgress(user, deptCode, topic.Id);
                response.Topics.Add(new TopicPb
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Description = topic.Description,
                    IconKey = topic.IconKey,
                    CategoryCode = category.Code,
                    TotalQuestions = Math.Min(TriviaCatalog.QuestionsPerSession, topic.Questions.Count),
                    Status = progress?.Status ?? TriviaMockDatabase.TopicNotStarted,
                    BestScore = progress?.BestScore ?? 0,
                    TimesCompleted = progress?.TimesCompleted ?? 0,
                    ResourceType = topic.Resource.Type,
                    IsDailyChallenge = string.Equals(topic.Id, dailyTopic.Id, StringComparison.OrdinalIgnoreCase)
                });
            }

            return Task.FromResult(new GetTopicsBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000
            });
        }

        // Pantalla 3: único recurso de aprendizaje del tema (pdf, imagen o video), previo
        // a iniciar la trivia. No consume intentos ni crea sesión.
        public override Task<GetTopicResourceBaseResponsePb> GetTopicResource(GetTopicResourceRequestPb request, ServerCallContext context)
        {
            var (_, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new GetTopicResourceBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var topic = TriviaCatalog.GetTopic(request.TopicId);
            if (topic == null)
            {
                return Task.FromResult(new GetTopicResourceBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR017,
                    Message = ErrorMessage.ERR017
                });
            }

            if (string.IsNullOrWhiteSpace(topic.Resource.Url))
            {
                return Task.FromResult(new GetTopicResourceBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR019,
                    Message = ErrorMessage.ERR019
                });
            }

            var response = new GetTopicResourceResponsePb
            {
                TopicId = topic.Id,
                TopicTitle = topic.Title,
                CategoryCode = topic.CategoryCode,
                CategoryLabel = topic.CategoryLabel,
                TotalQuestions = Math.Min(TriviaCatalog.QuestionsPerSession, topic.Questions.Count),
                IsDailyChallenge = TriviaCatalog.IsDailyChallengeTopic(topic.Id, BoliviaClock.Now),
                Resource = new TopicResourcePb
                {
                    Type = topic.Resource.Type,
                    Title = topic.Resource.Title,
                    Description = topic.Resource.Description,
                    Url = topic.Resource.Url
                }
            };

            return Task.FromResult(new GetTopicResourceBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000
            });
        }

        // Pantalla 4: banco de preguntas del tema. La sesión se crea aquí (y recién aquí se
        // descuenta un intento semanal).
        public override Task<GetCurrentQuestionBaseResponsePb> GetCurrentQuestion(GetCurrentQuestionRequestPb request, ServerCallContext context)
        {
            var (user, deptCode, errorCode, errorMsg) = ResolveUserAndDepartment(context.RequestHeaders, request.DepartmentCode);
            if (user == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            TriviaTopic? topic = null;
            TriviaSessionState? session = null;

            if (!string.IsNullOrEmpty(request.TriviaSessionId))
            {
                session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
                if (session == null)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR002,
                        Message = ErrorMessage.ERR002
                    });
                }
                if (session.IdUserProfile != user.IdUserProfile)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR015,
                        Message = ErrorMessage.ERR015
                    });
                }
                if (!string.Equals(session.DepartmentCode, deptCode, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR014,
                        Message = ErrorMessage.ERR014
                    });
                }
                if (!string.IsNullOrWhiteSpace(request.TopicId) &&
                    !string.Equals(session.TopicId, request.TopicId.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR018,
                        Message = ErrorMessage.ERR018
                    });
                }

                topic = TriviaCatalog.GetTopic(session.TopicId);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.TopicId))
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR020,
                        Message = ErrorMessage.ERR020
                    });
                }

                topic = TriviaCatalog.GetTopic(request.TopicId);
                if (topic == null)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR017,
                        Message = ErrorMessage.ERR017
                    });
                }

                session = TriviaMockDatabase.GetActiveSessionForTopic(user.IdUserProfile, deptCode, topic.Id);
            }

            if (topic == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR012,
                    Message = ErrorMessage.ERR012
                });
            }

            // If starting a new session, verify attempts and deduct
            if (session == null)
            {
                if (user.WeeklyAttemptsLeft <= 0)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR006,
                        Message = ErrorMessage.ERR006
                    });
                }

                user.WeeklyAttemptsLeft--;

                session = TriviaMockDatabase.StartNewSession(user.IdUserProfile, deptCode, topic);

                var topicProgress = TriviaMockDatabase.GetOrCreateTopicProgress(user, deptCode, topic);
                if (topicProgress.Status == TriviaMockDatabase.TopicNotStarted)
                {
                    topicProgress.Status = TriviaMockDatabase.TopicInProgress;
                }

                TriviaMockDatabase.RefreshDepartmentProgress(user, deptCode);
            }

            session.Touch();

            int totalQuestions = TriviaMockDatabase.GetSessionQuestionsCount(session);
            if (session.CurrentQuestionIndex >= totalQuestions)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR001,
                    Message = ErrorMessage.ERR001
                });
            }

            var mockQuestion = TriviaMockDatabase.GetQuestionForSession(session, session.CurrentQuestionIndex);
            if (mockQuestion == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR012,
                    Message = ErrorMessage.ERR012
                });
            }

            session.LastQuestionId = mockQuestion.Id;

            var response = new GetCurrentQuestionResponsePb
            {
                TriviaSessionId = session.SessionId,
                Department = session.DepartmentCode,
                CurrentQuestionNumber = session.CurrentQuestionIndex + 1,
                TotalQuestions = totalQuestions,
                TopicId = topic.Id,
                TopicTitle = topic.Title,
                CategoryCode = topic.CategoryCode,
                CategoryLabel = topic.CategoryLabel,
                Question = new TriviaQuestionPb
                {
                    Id = mockQuestion.Id,
                    Category = mockQuestion.Category,
                    CategoryLabel = mockQuestion.CategoryLabel,
                    CategoryIconKey = mockQuestion.CategoryIconKey,
                    TopicId = mockQuestion.TopicId,
                    TopicTitle = mockQuestion.TopicTitle,
                    Title = mockQuestion.Title
                }
            };

            foreach (var opt in mockQuestion.Options)
            {
                response.Question.Options.Add(new QuestionOptionPb
                {
                    Id = opt.Id,
                    Text = opt.Text
                });
            }

            var baseResponse = new GetCurrentQuestionBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000
            };

            return Task.FromResult(baseResponse);
        }

        public override Task<SubmitAnswerBaseResponsePb> SubmitAnswer(SubmitAnswerRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
            if (session == null)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = ErrorMessage.ERR002
                });
            }

            if (session.IdUserProfile != idUserProfile)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR015,
                    Message = ErrorMessage.ERR015
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = ErrorMessage.ERR010
                });
            }

            // Check session timeout
            if ((BoliviaClock.Now - session.LastAccessedAt) > TriviaMockDatabase.SessionTimeout)
            {
                TriviaMockDatabase.TerminateSession(session.SessionId);
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR005,
                    Message = ErrorMessage.ERR005
                });
            }

            // Everything below reads and mutates this session's counters, so it runs under
            // the session's lock — otherwise two concurrent submits for the same question
            // (double tap, client retry) could both read the same CurrentQuestionIndex and
            // each advance/score it independently.
            lock (session.Lock)
            {
                int totalQuestions = TriviaMockDatabase.GetSessionQuestionsCount(session);
                if (session.CurrentQuestionIndex >= totalQuestions)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR001,
                        Message = ErrorMessage.ERR001
                    });
                }

                var mockQuestion = TriviaMockDatabase.GetQuestionForSession(session, session.CurrentQuestionIndex);
                if (mockQuestion == null)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR012,
                        Message = ErrorMessage.ERR012
                    });
                }

                // Validate question_id matches current question index
                if (mockQuestion.Id != request.QuestionId)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR003,
                        Message = ErrorMessage.ERR003
                    });
                }

                // Validate selected_option_id belongs to the question
                bool optionExists = mockQuestion.Options.Any(opt => string.Equals(opt.Id, request.SelectedOptionId, StringComparison.OrdinalIgnoreCase));
                if (!optionExists)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR008,
                        Message = ErrorMessage.ERR008
                    });
                }

                bool isCorrect = string.Equals(mockQuestion.CorrectOptionId, request.SelectedOptionId, StringComparison.OrdinalIgnoreCase);
                int xpEarned = isCorrect ? 50 : 0;

                if (isCorrect)
                {
                    session.CorrectAnswersCount++;
                }

                // Advance session question pointer
                session.CurrentQuestionIndex++;
                session.Touch();

                bool isFinished = session.CurrentQuestionIndex >= totalQuestions;

                var response = new SubmitAnswerResponsePb
                {
                    IsCorrect = isCorrect,
                    CorrectOptionId = mockQuestion.CorrectOptionId,
                    Explanation = mockQuestion.Explanation,
                    IsSessionFinished = isFinished,
                    XpEarned = xpEarned,
                    CurrentQuestionNumber = session.CurrentQuestionIndex,
                    TotalQuestions = totalQuestions,
                    CorrectAnswersSoFar = session.CorrectAnswersCount
                };

                var baseResponse = new SubmitAnswerBaseResponsePb
                {
                    Data = response,
                    StatusCode = ErrorCode.SUC000
                };

                return Task.FromResult(baseResponse);
            }
        }

        public override Task<GetRewardsBaseResponsePb> GetRewards(GetRewardsRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
            if (session == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = ErrorMessage.ERR002
                });
            }

            if (session.IdUserProfile != idUserProfile)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR015,
                    Message = ErrorMessage.ERR015
                });
            }

            // Check session timeout
            if ((BoliviaClock.Now - session.LastAccessedAt) > TriviaMockDatabase.SessionTimeout)
            {
                TriviaMockDatabase.TerminateSession(session.SessionId);
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR005,
                    Message = ErrorMessage.ERR005
                });
            }

            int totalQuestions = TriviaMockDatabase.GetSessionQuestionsCount(session);
            if (session.CurrentQuestionIndex < totalQuestions)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR004,
                    Message = ErrorMessage.ERR004
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = ErrorMessage.ERR010
                });
            }

            var topic = TriviaCatalog.GetTopic(session.TopicId);
            if (topic == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR017,
                    Message = ErrorMessage.ERR017
                });
            }

            int score = session.CorrectAnswersCount;
            int xpFromQuestions = score * 50;
            int xpBonus = 100; // completion bonus
            int totalXpEarned = xpFromQuestions + xpBonus;
            int yastaCoinsEarned = score * 10;

            // Increment user total scores
            user.TotalXp += totalXpEarned;
            user.TotalYastaCoins += yastaCoinsEarned;

            // Cierra el tema: queda completado y se guarda el mejor puntaje histórico.
            var topicProgress = TriviaMockDatabase.GetOrCreateTopicProgress(user, session.DepartmentCode, topic);
            topicProgress.Status = TriviaMockDatabase.TopicCompleted;
            topicProgress.TimesCompleted++;
            topicProgress.LastCompletedAt = BoliviaClock.Now;
            if (score > topicProgress.BestScore)
            {
                topicProgress.BestScore = score;
            }

            // Los contadores del departamento se derivan del progreso por tema, así que se
            // recalculan aquí antes de evaluar el desbloqueo.
            TriviaMockDatabase.RefreshDepartmentProgress(user, session.DepartmentCode);

            var deptProgress = user.Departments[session.DepartmentCode];

            // Completar la sucursal de apertura sigue siendo lo que desbloquea el resto de
            // departamentos; se informan solo los que se desbloquearon en esta llamada.
            var unlockedDepts = new List<string>();
            if (deptProgress.Status == TriviaMockDatabase.DeptCompleted &&
                string.Equals(session.DepartmentCode, user.AccountOpeningBranch, StringComparison.OrdinalIgnoreCase))
            {
                var lockedBefore = user.Departments.Values
                    .Where(d => d.Status == TriviaMockDatabase.DeptLocked)
                    .Select(d => d.Code)
                    .ToList();

                TriviaMockDatabase.UnlockOtherDepartments(user);
                unlockedDepts.AddRange(lockedBefore);
            }

            var response = new GetRewardsResponsePb
            {
                SessionId = session.SessionId,
                Department = session.DepartmentCode,
                TopicId = topic.Id,
                TopicTitle = topic.Title,
                CategoryCode = topic.CategoryCode,
                CategoryLabel = topic.CategoryLabel,
                TopicStatus = topicProgress.Status,
                BestScore = topicProgress.BestScore,
                CompletedTopicsInDepartment = deptProgress.CompletedTopics,
                TotalTopicsInDepartment = deptProgress.TotalTopics,
                Score = new TriviaScorePb
                {
                    CorrectAnswers = score,
                    TotalQuestions = totalQuestions
                },
                RewardsEarned = new RewardsEarnedPb
                {
                    XpBonus = totalXpEarned,
                    YastaCoins = yastaCoinsEarned
                },
                Message = score >= 7
                    ? $"¡Excelente trabajo! Lograste un puntaje de {score}/{totalQuestions} en {topic.Title} y desbloqueaste altas recompensas."
                    : $"¡Buen intento! Lograste un puntaje de {score}/{totalQuestions} en {topic.Title}. ¡Sigue aprendiendo y mejora en la siguiente ronda!",
                WeeklyAttemptsLeft = user.WeeklyAttemptsLeft,
                DepartmentStatus = deptProgress.Status
            };

            response.UnlockedDepartmentCodes.AddRange(unlockedDepts);

            // Clean up session from database
            TriviaMockDatabase.TerminateSession(session.SessionId);

            var baseResponse = new GetRewardsBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000
            };

            return Task.FromResult(baseResponse);
        }
    }
}
