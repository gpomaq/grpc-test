namespace GrpcTest.Services
{
    // Recurso de aprendizaje que el usuario ve antes de entrar a la trivia del tema.
    // Cada tema expone exactamente uno (pdf, imagen o video), nunca una lista.
    public class TriviaResource
    {
        // "pdf" | "image" | "video"
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class TriviaTopic
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;

        // Rellenados por TriviaCatalog al construir el catálogo, para no repetir
        // la categoría en cada tema ni en cada pregunta del banco.
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryLabel { get; set; } = string.Empty;

        public TriviaResource Resource { get; set; } = new();
        public List<MockQuestion> Questions { get; set; } = new();
    }

    public class TriviaCategory
    {
        public string Code { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<TriviaTopic> Topics { get; set; } = new();
    }

    public class MockQuestion
    {
        public string Id { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryLabel { get; set; } = string.Empty;
        public string CategoryIconKey { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
        public string TopicTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<(string Id, string Text)> Options { get; set; } = new();
        public string CorrectOptionId { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }

    // Catálogo estático de contenido: categorías -> temas -> (recurso + banco de preguntas).
    // El estado por usuario (progreso, sesiones, intentos) vive en TriviaMockDatabase.
    public static class TriviaCatalog
    {
        // Cada sesión de trivia toma esta cantidad de preguntas del banco del tema,
        // elegidas al azar, de modo que repetir un tema no repita el mismo set.
        public const int QuestionsPerSession = 10;

        public static readonly IReadOnlyList<TriviaCategory> Categories;

        private static readonly Dictionary<string, TriviaCategory> CategoriesByCode;
        private static readonly Dictionary<string, TriviaTopic> TopicsById;

        public static readonly IReadOnlyList<TriviaTopic> AllTopics;

        public static readonly IReadOnlyDictionary<string, string> DepartmentNames = new Dictionary<string, string>
        {
            { "LP", "La Paz" }, { "SC", "Santa Cruz" }, { "CB", "Cochabamba" },
            { "OR", "Oruro" }, { "PT", "Potosí" }, { "TJ", "Tarija" },
            { "CH", "Chuquisaca" }, { "BE", "Beni" }, { "PD", "Pando" }
        };

        static TriviaCatalog()
        {
            Categories = QuestionBank.BuildCategories();

            // Denormaliza la categoría y el tema hacia abajo (temas y preguntas) una sola vez
            // al arrancar, para que el banco de preguntas solo declare su contenido propio.
            foreach (var category in Categories)
            {
                foreach (var topic in category.Topics)
                {
                    topic.CategoryCode = category.Code;
                    topic.CategoryLabel = category.Label;

                    foreach (var question in topic.Questions)
                    {
                        question.Category = category.Code;
                        question.CategoryLabel = category.Label;
                        question.CategoryIconKey = category.IconKey;
                        question.TopicId = topic.Id;
                        question.TopicTitle = topic.Title;
                    }
                }
            }

            CategoriesByCode = Categories.ToDictionary(c => c.Code, StringComparer.OrdinalIgnoreCase);
            AllTopics = Categories.SelectMany(c => c.Topics).ToList();
            TopicsById = AllTopics.ToDictionary(t => t.Id, StringComparer.OrdinalIgnoreCase);
        }

        public static TriviaCategory? GetCategory(string categoryCode)
        {
            if (string.IsNullOrWhiteSpace(categoryCode)) return null;
            return CategoriesByCode.TryGetValue(categoryCode.Trim(), out var category) ? category : null;
        }

        public static TriviaTopic? GetTopic(string topicId)
        {
            if (string.IsNullOrWhiteSpace(topicId)) return null;
            return TopicsById.TryGetValue(topicId.Trim(), out var topic) ? topic : null;
        }

        public static MockQuestion? GetQuestion(TriviaTopic topic, string questionId)
        {
            return topic.Questions.FirstOrDefault(q => q.Id == questionId);
        }

        public static bool IsValidDepartment(string departmentCode)
        {
            return !string.IsNullOrWhiteSpace(departmentCode) && DepartmentNames.ContainsKey(departmentCode.Trim().ToUpper());
        }

        public static int TotalTopicsCount => AllTopics.Count;

        // El desafío diario destaca un tema puntual y es el mismo para todos los usuarios
        // durante el día: se deriva de la fecha (hora Bolivia), no de un random, para que
        // la app y el backend siempre coincidan sin necesidad de persistirlo.
        // La categoría rota día a día y el tema dentro de ella avanza cada vuelta completa,
        // así dos días seguidos nunca caen en la misma categoría.
        public static TriviaTopic GetDailyChallengeTopic(DateTimeOffset now)
        {
            int daysSinceEpoch = (int)(now.Date - new DateTime(2024, 1, 1)).TotalDays;
            int day = ((daysSinceEpoch % AllTopics.Count) + AllTopics.Count) % AllTopics.Count;

            var category = Categories[day % Categories.Count];
            return category.Topics[(day / Categories.Count) % category.Topics.Count];
        }

        public static bool IsDailyChallengeTopic(string topicId, DateTimeOffset now)
        {
            return !string.IsNullOrWhiteSpace(topicId) &&
                   string.Equals(topicId.Trim(), GetDailyChallengeTopic(now).Id, StringComparison.OrdinalIgnoreCase);
        }

        // Helper usado por los archivos del banco de preguntas: genera los ids de opción
        // a partir del id de la pregunta ("<id>_a".."<id>_d") y marca la correcta por índice.
        internal static MockQuestion Q(string id, string title, string[] options, int correctIndex, string explanation)
        {
            var question = new MockQuestion
            {
                Id = id,
                Title = title,
                Explanation = explanation
            };

            for (int i = 0; i < options.Length; i++)
            {
                string optionId = $"{id}_{(char)('a' + i)}";
                question.Options.Add((optionId, options[i]));
                if (i == correctIndex)
                {
                    question.CorrectOptionId = optionId;
                }
            }

            return question;
        }
    }
}
