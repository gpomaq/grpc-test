namespace GrpcTest.Errors
{
    public struct ErrorCode
    {
        public const string SUC000 = nameof(SUC000);
        public const string VAL001 = nameof(VAL001);
        public const string ERR001 = nameof(ERR001); // Trivia session already finished
        public const string ERR002 = nameof(ERR002); // Trivia session not found
        public const string ERR003 = nameof(ERR003); // question_id mismatch with session's current question
        public const string ERR004 = nameof(ERR004); // Session not completed yet, cannot claim rewards
        public const string ERR005 = nameof(ERR005); // Session expired due to inactivity
        public const string ERR006 = nameof(ERR006); // Weekly attempts quota exhausted
        public const string ERR007 = nameof(ERR007); // Selected department is locked
        public const string ERR008 = nameof(ERR008); // Invalid selected_option_id
        public const string ERR009 = nameof(ERR009); // Authentication token not provided
        public const string ERR010 = nameof(ERR010); // id_user_profile not registered
        public const string ERR011 = nameof(ERR011); // id_user_profile claim missing from token
        public const string ERR012 = nameof(ERR012); // Current question could not be retrieved for the session
        public const string ERR013 = nameof(ERR013); // Invalid department code
        public const string ERR014 = nameof(ERR014); // department_code does not match the session's department
        public const string ERR015 = nameof(ERR015); // Session does not belong to the authenticated user
        public const string ERR016 = nameof(ERR016); // Invalid category code
        public const string ERR017 = nameof(ERR017); // Invalid topic id
        public const string ERR018 = nameof(ERR018); // topic_id does not match the session's topic
        public const string ERR019 = nameof(ERR019); // Topic has no learning resource configured
        public const string ERR020 = nameof(ERR020); // topic_id is required to start a new trivia session
    }
}
