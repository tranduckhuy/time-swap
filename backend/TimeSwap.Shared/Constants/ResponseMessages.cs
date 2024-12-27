namespace TimeSwap.Shared.Constants
{
    /// <summary>
    /// This section contains success messages for the API response.
    /// </summary>
    #region SuccessMessages
    public static class SuccessMessages
    {
        private static readonly Dictionary<SuccessCode, string> _messages = new Dictionary<SuccessCode, string>
        {
            { SuccessCode.Register, "Register success." },
            { SuccessCode.Login, "Login success." },
            { SuccessCode.Update, "Update success." }
        };

        public static string GetMessage(SuccessCode code) => _messages[code];
    }
    #endregion

    /// <summary>
    /// This section contains error messages for the API response.
    /// </summary>
    #region ErrorMessages
    public static class ErrorMessages
    {
        private static readonly Dictionary<ErrorCode, string> _messages = new Dictionary<ErrorCode, string>
        {
            { ErrorCode.RegisterFailed, "Register failed. Please try again." },
            { ErrorCode.LoginFailed, "Login failed. Invalid credentials." },
            { ErrorCode.ValidationFailed, "Validation failed. Please check your input." }
        };

        public static string GetMessage(ErrorCode code) => _messages[code];
    }
    #endregion
}
