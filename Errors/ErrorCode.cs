namespace GrpcTest.Errors
{
    public struct ErrorCode
    {
        public const string SUC000 = nameof(SUC000);
        public const string VAL001 = nameof(VAL001);
        public const string ERR001 = nameof(ERR001); // Trivia session already finished
        public const string ERR002 = nameof(ERR002); // Session or question not found
        public const string ERR003 = nameof(ERR003); // question_id mismatch with session's current question
        public const string ERR004 = nameof(ERR004); // Session not completed yet, cannot claim rewards
        public const string ERR005 = nameof(ERR005); // Session expired due to inactivity
        public const string ERR006 = nameof(ERR006); // Weekly attempts quota exhausted
        public const string ERR007 = nameof(ERR007); // Invalid or locked department
        public const string ERR008 = nameof(ERR008); // Invalid selected_option_id
        public const string ERR009 = nameof(ERR009); // Authentication/token error
        public const string ERR010 = nameof(ERR010); // id_user_profile not registered
    }
}
