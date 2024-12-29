namespace TimeSwap.Shared.Constants
{
    public enum StatusCode
    {
        /// <summary>
        /// Success Codes
        /// </summary>
        RequestProcessedSuccessfully = 1000,
        ConfirmationEmailSent = 1001,
        LoginSuccessful = 1002,
        ResetPasswordEmailSent = 1003,
        PasswordResetSuccessful = 1004,

        /// <summary>
        /// Error Codes 2xxx
        /// </summary>
        ModelInvalid = 2000,
        EmailAlreadyExists = 2001,
        RegisterFailed = 2002,
        UserNotExists = 2003,
        UserNotConfirmed = 2004,
        InvalidCredentials = 2005,
        UserAuthenticationFailed = 2006,
        ConfirmEmailTokenInvalidOrExpired = 2007,
        UserAlreadyConfirmed = 2008,
        InvalidToken = 2009,
        ProvidedInformationIsInValid = 2010,
        TokenIsBlacklisted = 2011,
    }
}
