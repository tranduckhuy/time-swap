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
        RequestProcessingFailed = 2000,
        ModelInvalid = 2001,
        EmailAlreadyExists = 2002,
        RegisterFailed = 2003,
        UserNotExists = 2004,
        UserNotConfirmed = 2005,
        InvalidCredentials = 2006,
        UserAuthenticationFailed = 2007,
        ConfirmEmailTokenInvalidOrExpired = 2008,
        UserAlreadyConfirmed = 2009,
        InvalidToken = 2010,
        ProvidedInformationIsInValid = 2011,
        TokenIsBlacklisted = 2012,
        IndustryNotFound = 2013,
        IndustrySameName = 2014,
        CategoryNotFound = 2015,
        CategoryNotFoundByIndustryId = 2016
    }
}
