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
    }

    public class TriviaSessionState
    {
        public string SessionId { get; set; } = string.Empty;
        public string IdUserProfile { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; } = 0;
        public int CorrectAnswersCount { get; set; } = 0;
        public string LastQuestionId { get; set; } = string.Empty;
        public DateTimeOffset LastAccessedAt { get; set; } = BoliviaClock.Now;

        // Guards read-modify-write of this session's mutable counters (e.g. concurrent
        // SubmitAnswer retries) so a double request can't advance/score a question twice.
        public readonly object Lock = new();

        public void Touch() => LastAccessedAt = BoliviaClock.Now;
    }

    public class MockQuestion
    {
        public string Id { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryLabel { get; set; } = string.Empty;
        public string CategoryIconKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<(string Id, string Text)> Options { get; set; } = new();
        public string CorrectOptionId { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }

    public static class TriviaMockDatabase
    {
        private static readonly ConcurrentDictionary<string, UserState> Users = new();
        private static readonly ConcurrentDictionary<string, TriviaSessionState> Sessions = new();
        private static readonly ConcurrentDictionary<string, string> DepartmentSessions = new();

        public static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(30);

        // Predefined list of 10 mixed questions (Finance, Environment, Gender Equity, Women's Rights)
        private static readonly List<MockQuestion> MockQuestions = new()
        {
            new MockQuestion
            {
                Id = "q1",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Cuál es el principal beneficio del interés compuesto al ahorrar en un banco boliviano?",
                Options = new()
                {
                    ("opt1a", "Calcula intereses únicamente sobre el monto del depósito inicial."),
                    ("opt1b", "Genera intereses sobre el capital inicial y también sobre los intereses acumulados previamente."),
                    ("opt1c", "Garantiza que la inflación del país nunca afectará el valor real de tus ahorros."),
                    ("opt1d", "Se aplica únicamente en préstamos de consumo a muy corto plazo.")
                },
                CorrectOptionId = "opt1b",
                Explanation = "El interés compuesto suma los intereses ganados al capital inicial de forma periódica, haciendo que el interés futuro se calcule sobre un monto mayor."
            },
            new MockQuestion
            {
                Id = "q2",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "En Bolivia, ¿cuál de las siguientes opciones representa una fuente de energía renovable clave en pleno desarrollo en el altiplano?",
                Options = new()
                {
                    ("opt2a", "El carbón mineral."),
                    ("opt2b", "La energía solar fotovoltaica (como en la planta solar de Oruro)."),
                    ("opt2c", "El gas natural licuado."),
                    ("opt2d", "La energía de fisión nuclear.")
                },
                CorrectOptionId = "opt2b",
                Explanation = "Bolivia cuenta con plantas de energía solar (como la de Oruro) que aprovechan la alta radiación solar del altiplano para generar energía limpia y renovable."
            },
            new MockQuestion
            {
                Id = "q3",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Cuál es el propósito central de promover la equidad de género en las empresas e instituciones en Bolivia?",
                Options = new()
                {
                    ("opt3a", "Garantizar igualdad de oportunidades, trato y remuneración justa sin importar el género."),
                    ("opt3b", "Establecer que los hombres trabajen turnos más largos en áreas operativas."),
                    ("opt3c", "Restringir la participación de mujeres en puestos jerárquicos o de toma de decisiones."),
                    ("opt3d", "Dividir las tareas de oficina estrictamente bajo roles tradicionales de género.")
                },
                CorrectOptionId = "opt3a",
                Explanation = "La equidad de género busca eliminar barreras históricas para asegurar oportunidades iguales y salarios justos por el mismo trabajo, beneficiando a toda la sociedad boliviana."
            },
            new MockQuestion
            {
                Id = "q4",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "En Bolivia, ¿qué ley nacional garantiza a las mujeres una vida libre de violencia y tipifica el delito de feminicidio?",
                Options = new()
                {
                    ("opt4a", "La Ley General del Trabajo."),
                    ("opt4b", "La Ley N° 348 (Ley Integral para Garantizar a las Mujeres una Vida Libre de Violencia)."),
                    ("opt4c", "El Código de las Familias."),
                    ("opt4d", "La Ley N° 004 de Lucha contra la Corrupción.")
                },
                CorrectOptionId = "opt4b",
                Explanation = "La Ley N° 348 es la norma integral específica que protege a las mujeres contra todo tipo de violencia física, psicológica, sexual o económica en Bolivia."
            },
            new MockQuestion
            {
                Id = "q5",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "Para una familia boliviana, ¿cuál es el objetivo primordial de elaborar un presupuesto mensual en bolivianos (Bs.)?",
                Options = new()
                {
                    ("opt5a", "Maximizar el límite de uso de las tarjetas de crédito."),
                    ("opt5b", "Planificar y controlar los ingresos frente a los gastos, priorizando el ahorro y pago de deudas."),
                    ("opt5c", "Evitar por completo cualquier tipo de consumo cultural o recreativo familiar."),
                    ("opt5d", "Reportar todos los consumos directamente al Servicio de Impuestos Nacionales.")
                },
                CorrectOptionId = "opt5b",
                Explanation = "Un presupuesto permite ordenar las finanzas familiares, asegurando que se cubran las necesidades básicas y se destine un porcentaje al ahorro antes de gastar."
            },
            new MockQuestion
            {
                Id = "q6",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es una de las principales amenazas para la biodiversidad del Parque Nacional Madidi en Bolivia?",
                Options = new()
                {
                    ("opt6a", "La reforestación con árboles nativos de la Amazonía."),
                    ("opt6b", "La deforestación ilegal y la contaminación de ríos por minería aurífera sin control."),
                    ("opt6c", "El incremento del turismo ecológico regulado y sostenible."),
                    ("opt6d", "El desarrollo de técnicas agrícolas tradicionales de rotación de cultivos.")
                },
                CorrectOptionId = "opt6b",
                Explanation = "La minería aurífera ilegal que libera mercurio en los ríos y la deforestación representan graves peligros para la fauna y comunidades indígenas del Madidi."
            },
            new MockQuestion
            {
                Id = "q7",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "En el mercado laboral boliviano, ¿qué describe el concepto de 'brecha salarial de género'?",
                Options = new()
                {
                    ("opt7a", "La diferencia promedio en los ingresos percibidos por hombres y mujeres que realizan trabajos de igual valor."),
                    ("opt7b", "La diferencia de edad promedio en la que se jubilan los hombres y las mujeres bolivianas."),
                    ("opt7c", "La cantidad de feriados anuales que corresponden por ley a cada género."),
                    ("opt7d", "La brecha en la cantidad de horas destinadas al descanso semanal.")
                },
                CorrectOptionId = "opt7a",
                Explanation = "La brecha salarial es el indicador que muestra que, en promedio, las mujeres ganan menos que los hombres por realizar trabajos con similares responsabilidades."
            },
            new MockQuestion
            {
                Id = "q8",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿A qué derecho de la mujer boliviana hace referencia específica el principio constitucional de 'paridad y alternancia' en cargos de elección política?",
                Options = new()
                {
                    ("opt8a", "Al derecho a la salud reproductiva."),
                    ("opt8b", "Al derecho a la participación política equitativa en la toma de decisiones y representación pública."),
                    ("opt8c", "Al derecho a percibir el subsidio de lactancia maternal."),
                    ("opt8d", "Al derecho de acceso prioritario a créditos de vivienda social.")
                },
                CorrectOptionId = "opt8b",
                Explanation = "La paridad y alternancia garantizan que las listas de candidatos políticos en Bolivia tengan una distribución equitativa de 50% mujeres y 50% hombres de forma intercalada."
            },
            new MockQuestion
            {
                Id = "q9",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es el beneficio ambiental más directo de reciclar envases plásticos y latas en nuestras ciudades bolivianas?",
                Options = new()
                {
                    ("opt9a", "Incrementar la temperatura en las zonas urbanas."),
                    ("opt9b", "Reducir la saturación de los vertederos municipales (como Alpacoma o Kara Kara) y ahorrar materias primas."),
                    ("opt9c", "Generar lluvias más frecuentes en los valles del país."),
                    ("opt9d", "Eliminar la necesidad de tratamiento de agua potable.")
                },
                CorrectOptionId = "opt9b",
                Explanation = "Al reciclar, evitamos que estos materiales tarden cientos de años en degradarse en los saturados rellenos sanitarios de ciudades como La Paz o Cochabamba."
            },
            new MockQuestion
            {
                Id = "q10",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "En Bolivia, ¿cuál de las siguientes opciones describe la duración legal de la licencia de maternidad pagada para madres asalariadas?",
                Options = new()
                {
                    ("opt10a", "Un total de 30 días calendario únicamente."),
                    ("opt10b", "Un total de 90 días calendario (45 días antes del parto y 45 días después)."),
                    ("opt10c", "Un total de 180 días calendario."),
                    ("opt10d", "La licencia no tiene una duración fija y depende del acuerdo con el empleador.")
                },
                CorrectOptionId = "opt10b",
                Explanation = "La legislación boliviana otorga a las madres trabajadoras una licencia de maternidad remunerada de 90 días en total, divididos en etapas prenatal y postnatal, garantizando su estabilidad laboral."
            }
        };

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
                        var deptEntries = DepartmentSessions.Where(d => d.Value == id).Select(d => d.Key).ToList();
                        foreach (var dept in deptEntries)
                        {
                            DepartmentSessions.TryRemove(dept, out var _);
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
            var deptCodes = new[] { "LP", "SC", "CB", "OR", "PT", "TJ", "CH", "BE", "PD" };
            var deptNames = new Dictionary<string, string>
            {
                { "LP", "La Paz" }, { "SC", "Santa Cruz" }, { "CB", "Cochabamba" },
                { "OR", "Oruro" }, { "PT", "Potosí" }, { "TJ", "Tarija" },
                { "CH", "Chuquisaca" }, { "BE", "Beni" }, { "PD", "Pando" }
            };

            foreach (var code in deptCodes)
            {
                bool isOpening = string.Equals(code, user.AccountOpeningBranch, StringComparison.OrdinalIgnoreCase);
                var dept = new DepartmentProgressPb
                {
                    Code = code,
                    Name = deptNames[code],
                    Status = isOpening ? "unlocked" : "locked",
                    CompletedQuestions = 0,
                    TotalQuestions = MockQuestions.Count
                };
                user.Departments[code] = dept;
            }
        }

        public static void UnlockOtherDepartments(UserState user)
        {
            foreach (var kv in user.Departments)
            {
                if (kv.Value.Status == "locked")
                {
                    kv.Value.Status = "unlocked";
                }
            }
        }

        public static TriviaSessionState? GetSession(string sessionId)
        {
            if (Sessions.TryGetValue(sessionId, out var session))
            {
                return session;
            }
            return null;
        }

        public static TriviaSessionState? GetActiveSessionForDepartment(string idUserProfile, string departmentCode)
        {
            string key = $"{idUserProfile}_{departmentCode.ToUpper()}";
            if (DepartmentSessions.TryGetValue(key, out string? sessionId))
            {
                var session = GetSession(sessionId);
                if (session != null && session.CurrentQuestionIndex < MockQuestions.Count)
                {
                    return session;
                }
            }
            return null;
        }

        public static TriviaSessionState StartNewSession(string idUserProfile, string departmentCode)
        {
            string sessionId = Guid.NewGuid().ToString();
            var session = new TriviaSessionState
            {
                SessionId = sessionId,
                IdUserProfile = idUserProfile,
                DepartmentCode = departmentCode.ToUpper(),
                CurrentQuestionIndex = 0,
                CorrectAnswersCount = 0
            };

            Sessions[sessionId] = session;
            string key = $"{idUserProfile}_{departmentCode.ToUpper()}";
            DepartmentSessions[key] = sessionId;

            return session;
        }

        public static void TerminateSession(string sessionId)
        {
            if (Sessions.TryRemove(sessionId, out var session))
            {
                string key = $"{session.IdUserProfile}_{session.DepartmentCode}";
                DepartmentSessions.TryRemove(key, out _);
            }
        }

        public static List<MockQuestion> GetQuestions() => MockQuestions;

        public static MockQuestion? GetQuestion(string questionId)
        {
            return MockQuestions.FirstOrDefault(q => q.Id == questionId);
        }

        public static MockQuestion? GetQuestionByIndex(int index)
        {
            if (index >= 0 && index < MockQuestions.Count)
                return MockQuestions[index];
            return null;
        }

        public static int GetTotalQuestionsCount() => MockQuestions.Count;
    }
}
