using System.Collections.Concurrent;
using Grpc.Core;

namespace GrpcTest.Services;

public class TriviaService : GrpcTest.TriviaService.TriviaServiceBase
{
    // Thread-safe dictionary to keep track of active sessions in memory
    private static readonly ConcurrentDictionary<string, TriviaSessionState> Sessions = new();

    private class TriviaSessionState
    {
        public string SessionId { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; }
        public int CorrectAnswersCount { get; set; }
        public string LastQuestionId { get; set; } = string.Empty;
    }

    private class MockQuestion
    {
        public string Id { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<(string Id, string Text)> Options { get; set; } = [];
        public string CorrectOptionId { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }

    // A static list of exactly 10 questions in Spanish, contextualized for Bolivia.
    // It guarantees variety: Finanzas, Medio Ambiente, and Equidad de Género.
    private static readonly List<MockQuestion> MockQuestions =
    [
        new MockQuestion
        {
            Id = "q1",
            Category = "finanzas",
            Title = "¿Cuál es el principal beneficio del interés compuesto al ahorrar en un banco boliviano?",
            Options =
            [
                ("opt1a", "Calcula intereses únicamente sobre el monto del depósito inicial."),
                ("opt1b", "Genera intereses sobre el capital inicial y también sobre los intereses acumulados previamente."),
                ("opt1c", "Garantiza que la inflación del país nunca afectará el valor real de tus ahorros."),
                ("opt1d", "Se aplica únicamente en préstamos de consumo a muy corto plazo.")
            ],
            CorrectOptionId = "opt1b",
            Explanation = "El interés compuesto suma los intereses ganados al capital inicial de forma periódica, haciendo que el interés futuro se calcule sobre un monto mayor."
        },
        new MockQuestion
        {
            Id = "q2",
            Category = "medio_ambiente",
            Title = "En Bolivia, ¿cuál de las siguientes opciones representa una fuente de energía renovable clave en pleno desarrollo en el altiplano?",
            Options =
            [
                ("opt2a", "El carbón mineral."),
                ("opt2b", "La energía solar fotovoltaica (como en la planta solar de Oruro)."),
                ("opt2c", "El gas natural licuado."),
                ("opt2d", "La energía de fisión nuclear.")
            ],
            CorrectOptionId = "opt2b",
            Explanation = "Bolivia cuenta con plantas de energía solar (como la de Oruro) que aprovechan la alta radiación solar del altiplano para generar energía limpia y renovable."
        },
        new MockQuestion
        {
            Id = "q3",
            Category = "equidad_genero",
            Title = "¿Cuál es el propósito central de promover la equidad de género en las empresas e instituciones en Bolivia?",
            Options =
            [
                ("opt3a", "Garantizar igualdad de oportunidades, trato y remuneración justa sin importar el género."),
                ("opt3b", "Establecer que los hombres trabajen turnos más largos en áreas operativas."),
                ("opt3c", "Restringir la participación de mujeres en puestos jerárquicos o de toma de decisiones."),
                ("opt3d", "Dividir las tareas de oficina estrictamente bajo roles tradicionales de género.")
            ],
            CorrectOptionId = "opt3a",
            Explanation = "La equidad de género busca eliminar barreras históricas para asegurar oportunidades iguales y salarios justos por el mismo trabajo, beneficiando a toda la sociedad boliviana."
        },
        new MockQuestion
        {
            Id = "q4",
            Category = "finanzas",
            Title = "Para una familia boliviana, ¿cuál es el objetivo primordial de elaborar un presupuesto mensual en bolivianos (Bs.)?",
            Options =
            [
                ("opt4a", "Maximizar el límite de uso de las tarjetas de crédito."),
                ("opt4b", "Planificar y controlar los ingresos frente a los gastos, priorizando el ahorro y pago de deudas."),
                ("opt4c", "Evitar por completo cualquier tipo de consumo cultural o recreativo familiar."),
                ("opt4d", "Reportar todos los consumos directamente al Servicio de Impuestos Nacionales.")
            ],
            CorrectOptionId = "opt4b",
            Explanation = "Un presupuesto permite ordenar las finanzas familiares, asegurando que se cubran las necesidades básicas y se destine un porcentaje al ahorro antes de gastar."
        },
        new MockQuestion
        {
            Id = "q5",
            Category = "medio_ambiente",
            Title = "¿Cuál es una de las principales amenazas para la biodiversidad del Parque Nacional Madidi en Bolivia?",
            Options =
            [
                ("opt5a", "La reforestación con árboles nativos de la Amazonía."),
                ("opt5b", "La deforestación ilegal y la contaminación de ríos por minería aurífera sin control."),
                ("opt5c", "El incremento del turismo ecológico regulado y sostenible."),
                ("opt5d", "El desarrollo de técnicas agrícolas tradicionales de rotación de cultivos.")
            ],
            CorrectOptionId = "opt5b",
            Explanation = "La minería aurífera ilegal que libera mercurio en los ríos y la deforestación representan graves peligros para la fauna y comunidades indígenas del Madidi."
        },
        new MockQuestion
        {
            Id = "q6",
            Category = "equidad_genero",
            Title = "En el mercado laboral boliviano, ¿qué describe el concepto de 'brecha salarial de género'?",
            Options =
            [
                ("opt6a", "La diferencia promedio en los ingresos percibidos por hombres y mujeres que realizan trabajos de igual valor."),
                ("opt6b", "La diferencia de edad promedio en la que se jubilan los hombres y las mujeres bolivianas."),
                ("opt6c", "La cantidad de feriados anuales que corresponden por ley a cada género."),
                ("opt6d", "La brecha en la cantidad de horas destinadas al descanso semanal.")
            ],
            CorrectOptionId = "opt6a",
            Explanation = "La brecha salarial es el indicador que muestra que, en promedio, las mujeres ganan menos que los hombres por realizar trabajos con similares responsabilidades."
        },
        new MockQuestion
        {
            Id = "q7",
            Category = "finanzas",
            Title = "Si vives en Bolivia, ¿cuántos meses de gastos básicos se recomienda tener acumulados en tu Fondo de Emergencia?",
            Options =
            [
                ("opt7a", "El equivalente a una semana de gastos únicamente."),
                ("opt7b", "Entre 3 y 6 meses de tus gastos indispensables de subsistencia."),
                ("opt7c", "El equivalente al valor total de un vehículo nuevo de importación."),
                ("opt7d", "No es necesario tener fondos acumulados si se cuenta con tarjetas de crédito.")
            ],
            CorrectOptionId = "opt7b",
            Explanation = "Contar con un fondo de 3 a 6 meses de gastos te protege frente a imprevistos graves como la pérdida de empleo, problemas de salud o reparaciones mayores sin caer en deudas."
        },
        new MockQuestion
        {
            Id = "q8",
            Category = "medio_ambiente",
            Title = "¿Cuál es el beneficio ambiental más directo de reciclar envases plásticos y latas en nuestras ciudades bolivianas?",
            Options =
            [
                ("opt8a", "Incrementar la temperatura en las zonas urbanas."),
                ("opt8b", "Reducir la saturación de los vertederos municipales (como Alpacoma o Kara Kara) y ahorrar materias primas."),
                ("opt8c", "Generar lluvias más frecuentes en los valles del país."),
                ("opt8d", "Eliminar la necesidad de tratamiento de agua potable.")
            ],
            CorrectOptionId = "opt8b",
            Explanation = "Al reciclar, evitamos que estos materiales tarden cientos de años en degradarse en los saturados rellenos sanitarios de ciudades como La Paz o Cochabamba."
        },
        new MockQuestion
        {
            Id = "q9",
            Category = "equidad_genero",
            Title = "En Bolivia, ¿a qué se refiere el concepto de 'doble jornada laboral' que afecta principalmente a las mujeres?",
            Options =
            [
                ("opt9a", "A tener dos empleos de tiempo completo contratados formalmente en distintas empresas."),
                ("opt9b", "A la combinación del trabajo remunerado fuera del hogar con las tareas domésticas y de cuidado no remuneradas."),
                ("opt9c", "Al pago por horas extra trabajadas durante los fines de semana y feriados nacionales."),
                ("opt9d", "A la realización de guardias nocturnas obligatorias en sectores de salud o seguridad.")
            ],
            CorrectOptionId = "opt9b",
            Explanation = "La doble jornada describe cómo las mujeres asumen la mayor parte del trabajo doméstico y de cuidado no remunerado, sumado a su empleo laboral diario."
        },
        new MockQuestion
        {
            Id = "q10",
            Category = "finanzas",
            Title = "¿Qué entidad en Bolivia es la encargada de regular, supervisar y controlar el sistema financiero (bancos, cooperativas, etc.)?",
            Options =
            [
                ("opt10a", "El Banco Central de Bolivia (BCB) exclusivamente."),
                ("opt10b", "La Autoridad de Supervisión del Sistema Financiero (ASFI)."),
                ("opt10c", "El Servicio de Impuestos Nacionales (SIN)."),
                ("opt10d", "La Bolsa Boliviana de Valores (BBV).")
            ],
            CorrectOptionId = "opt10b",
            Explanation = "La ASFI es la institución del Estado encargada de velar por la solidez del sistema financiero y defender los derechos de los consumidores financieros en Bolivia."
        }
    ];

    public override Task<GetProfileBaseResponsePb> GetProfile(GetProfileRequestPb request, ServerCallContext context)
    {
        var response = new GetProfileResponsePb
        {
            User = new UserProfilePb
            {
                Id = "user_12345",
                Name = "John Doe",
                Gender = "male",
                Age = 28,
                AccountOpeningBranch = "LP" // Código de departamento de apertura
            },
            Quota = new UserQuotaPb
            {
                WeeklyAttemptsMax = 3,
                WeeklyAttemptsLeft = 2,
                NextResetDate = DateTime.Now.AddDays(4).ToString("o") // ISO 8601
            },
            GlobalProgress = new GlobalProgressPb
            {
                TotalXp = 450,
                TotalYastaCoins = 85
            }
        };

        // Listado de departamentos (LP, SC, TJ, CB, OR, PT, CH, BE, PD)
        // Según reglas de negocio: LP (apertura) está completado, por tanto el resto del mapa se desbloquea.
        var depts = new List<DepartmentProgressPb>
        {
            new() { Code = "LP", Name = "La Paz", Status = "completed", CompletedQuestions = 10, TotalQuestions = 10 },
            new() { Code = "SC", Name = "Santa Cruz", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 },
            new() { Code = "TJ", Name = "Tarija", Status = "in_progress", CompletedQuestions = 4, TotalQuestions = 10 },
            new() { Code = "CB", Name = "Cochabamba", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 },
            new() { Code = "OR", Name = "Oruro", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 },
            new() { Code = "PT", Name = "Potosí", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 },
            new() { Code = "CH", Name = "Chuquisaca", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 },
            new() { Code = "BE", Name = "Beni", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 },
            new() { Code = "PD", Name = "Pando", Status = "unlocked", CompletedQuestions = 0, TotalQuestions = 10 }
        };

        response.GlobalProgress.Departments.AddRange(depts);

        var baseResponse = new GetProfileBaseResponsePb
        {
            Data = response,
            StatusCode = "SUC000"
        };

        return Task.FromResult(baseResponse);
    }

    public override Task<GetCurrentQuestionBaseResponsePb> GetCurrentQuestion(GetCurrentQuestionRequestPb request, ServerCallContext context)
    {
        var deptCode = string.IsNullOrWhiteSpace(request.DepartmentCode) ? "LP" : request.DepartmentCode.ToUpper();

        // Buscar si existe sesión en curso para este departamento, o crear una nueva.
        var session = Sessions.Values.FirstOrDefault(s => s.DepartmentCode == deptCode && s.CurrentQuestionIndex < MockQuestions.Count);

        if (session == null)
        {
            var newSessionId = Guid.NewGuid().ToString();
            session = new TriviaSessionState
            {
                SessionId = newSessionId,
                DepartmentCode = deptCode,
                CurrentQuestionIndex = 0,
                CorrectAnswersCount = 0
            };
            Sessions[newSessionId] = session;
        }

        var currentQIndex = session.CurrentQuestionIndex;
        var currentMockQ = MockQuestions[currentQIndex];
        session.LastQuestionId = currentMockQ.Id;

        var response = new GetCurrentQuestionResponsePb
        {
            TriviaSessionId = session.SessionId,
            Department = session.DepartmentCode,
            CurrentQuestionNumber = currentQIndex + 1,
            TotalQuestions = MockQuestions.Count,
            Question = new TriviaQuestionPb
            {
                Id = currentMockQ.Id,
                Category = currentMockQ.Category,
                Title = currentMockQ.Title
            }
        };

        foreach (var opt in currentMockQ.Options)
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
            StatusCode = "SUC000"
        };

        return Task.FromResult(baseResponse);
    }

    public override Task<SubmitAnswerBaseResponsePb> SubmitAnswer(SubmitAnswerRequestPb request, ServerCallContext context)
    {
        if (!Sessions.TryGetValue(request.TriviaSessionId, out var session))
        {
            //throw new RpcException(new Status(StatusCode.NotFound, $"La sesión de trivia '{request.TriviaSessionId}' no fue encontrada."));
            return Task.FromResult(new SubmitAnswerBaseResponsePb
            {
                StatusCode = "ERR002",
                Message = "La sesión de trivia no fue encontrada.",
            });
        }

        if (session.CurrentQuestionIndex >= MockQuestions.Count)
        {
            return Task.FromResult(new SubmitAnswerBaseResponsePb
            {
                StatusCode = "ERR001",
                Message = "La sesión ya ha finalizado. Por favor reclame sus recompensas.",
            });
        }

        var currentMockQ = MockQuestions[session.CurrentQuestionIndex];

        // Validar que coincida el ID de la pregunta
        if (currentMockQ.Id != request.QuestionId)
        {
            return Task.FromResult(new SubmitAnswerBaseResponsePb
            {
                StatusCode = "ERR003",
                Message = "Id de pregunta incorrecto.",
            });
        }

        var isCorrect = string.Equals(currentMockQ.CorrectOptionId, request.SelectedOptionId, StringComparison.OrdinalIgnoreCase);

        if (isCorrect)
        {
            session.CorrectAnswersCount++;
        }

        session.CurrentQuestionIndex++;
        var isFinished = session.CurrentQuestionIndex >= MockQuestions.Count;

        var response = new SubmitAnswerResponsePb
        {
            IsCorrect = isCorrect,
            CorrectOptionId = currentMockQ.CorrectOptionId,
            Explanation = currentMockQ.Explanation,
            IsSessionFinished = isFinished
        };

        var baseResponse = new SubmitAnswerBaseResponsePb
        {
            Data = response,
            StatusCode = "SUC000"
        };

        return Task.FromResult(baseResponse);
    }

    public override Task<GetRewardsBaseResponsePb> GetRewards(GetRewardsRequestPb request, ServerCallContext context)
    {
        if (!Sessions.TryGetValue(request.TriviaSessionId, out var session))
        {
            return Task.FromResult(new GetRewardsBaseResponsePb
            {
                StatusCode = "ERR002",
                Message = "La sesión de trivia no fue encontrada.",
            });
        }

        if (session.CurrentQuestionIndex < MockQuestions.Count)
        {
            return Task.FromResult(new GetRewardsBaseResponsePb
            {
                StatusCode = "ERR004",
                Message = "La sesión de trivia aún no ha sido completada.",
            });
        }

        var score = session.CorrectAnswersCount;
        var xpBonus = score * 50;         // 50 XP por respuesta correcta
        var yastaCoins = score * 10;      // 10 yasta_coins por respuesta correcta

        var response = new GetRewardsResponsePb
        {
            SessionId = session.SessionId,
            Department = session.DepartmentCode,
            Score = new TriviaScorePb
            {
                CorrectAnswers = score,
                TotalQuestions = MockQuestions.Count
            },
            RewardsEarned = new RewardsEarnedPb
            {
                XpBonus = xpBonus,
                YastaCoins = yastaCoins
            },
            Message = score >= 7
                ? $"¡Excelente trabajo! Lograste un puntaje de {score}/{MockQuestions.Count} y desbloqueaste altas recompensas."
                : $"¡Buen intento! Lograste un puntaje de {score}/{MockQuestions.Count}. ¡Sigue aprendiendo y mejora en la siguiente ronda!"
        };

        // Remover la sesión para no fugar memoria en el servidor mock
        Sessions.TryRemove(request.TriviaSessionId, out _);

        var baseResponse = new GetRewardsBaseResponsePb
        {
            Data = response,
            StatusCode = "SUC000"
        };

        return Task.FromResult(baseResponse);
    }
}
