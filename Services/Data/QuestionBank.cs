namespace GrpcTest.Services
{
    // Banco de contenido del mock. Cada categoría vive en su propio archivo parcial
    // (QuestionBank.<Categoria>.cs) para que agregar temas o preguntas no obligue a
    // tocar un único archivo gigante.
    public static partial class QuestionBank
    {
        internal static IReadOnlyList<TriviaCategory> BuildCategories() => new List<TriviaCategory>
        {
            BuildEducacionFinanciera(),
            BuildMedioAmbiente(),
            BuildEquidadGenero(),
            BuildDerechosMujer()
        };
    }
}
