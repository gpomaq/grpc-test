namespace GrpcTest.Errors
{
    public struct ErrorMessage
    {
        // Success responses carry no message, only data.
        public const string SUC000 = "";

        public const string VAL001 = "Se produjeron uno o más errores de validación.";

        // Used only by BaseResponse<T>.Exception() (generic/unused infra), kept as-is.
        public const string ERR001_LEGACY_EXCEPTION = "¡Oh no! Parece que hubo un problema al completar tu solicitud. Por favor, vuelve más tarde.";

        public const string ERR001 = "La sesión ya ha finalizado. Por favor reclame sus recompensas.";
        public const string ERR002 = "La sesión de trivia no fue encontrada.";
        public const string ERR003 = "La pregunta enviada no corresponde al orden actual de tu sesión de trivia.";
        public const string ERR004 = "La sesión de trivia aún no ha sido completada. Debes responder todas las preguntas antes de reclamar tus recompensas.";
        public const string ERR005 = "La sesión de trivia ha expirado por inactividad.";
        public const string ERR006 = "Has agotado tus intentos semanales permitidos para jugar trivias.";
        public const string ERR007 = "El departamento seleccionado está bloqueado. Debes completar primero tu departamento de apertura.";
        public const string ERR008 = "La opción seleccionada es inválida o no corresponde a la pregunta.";
        public const string ERR009 = "No autenticado. Token de autenticación no proporcionado.";
        public const string ERR010 = "No se encontró un usuario registrado con el id_user_profile del token.";
        public const string ERR011 = "No autenticado. El claim 'id_user_profile' no se encuentra en el token.";
        public const string ERR012 = "No se pudo recuperar la pregunta de la sesión de trivia.";
        public const string ERR013 = "El código de departamento no es válido.";
        public const string ERR014 = "El department_code no corresponde al departamento de la sesión de trivia especificada.";
        public const string ERR015 = "La sesión de trivia especificada no pertenece al usuario autenticado.";
        public const string ERR016 = "La categoría no es válida.";
        public const string ERR017 = "El tema no es válido.";
        public const string ERR018 = "El topic_id no corresponde al tema de la sesión de trivia especificada.";
        public const string ERR019 = "No se encontró un recurso de aprendizaje para el tema.";
        public const string ERR020 = "Debe especificar un topic_id para iniciar una nueva sesión de trivia.";
    }
}
