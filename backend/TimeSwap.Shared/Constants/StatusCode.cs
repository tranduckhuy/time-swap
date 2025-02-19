﻿namespace TimeSwap.Shared.Constants
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
        WardNotFound = 2016,
        CityNotFound = 2017,
        JobPostNotFound = 2018,
        InvalidWardInCity = 2019,
        WardIdRequireWhenCityIdProvided = 2020,
        CityIdRequireWhenWardIdProvided = 2021,
        InvalidCategoryInIndustry = 2022,
        UserNotEnoughBalance = 2023,
        DueDateMustBeGreaterThanStartDate = 2024,
        DueDateMustBeGreaterThanCurrentDate = 2025,
        FeeMustBeGreaterThanFiftyThousand = 2026,
        UserNotAppliedToJobPost = 2027,
        JobPostAlreadyAssigned = 2028,
        AssignJobToOwner = 2029,
        OwnerJobPostMismatch = 2030,
        UserAppliedToOwnJobPost = 2031,
        PaymentMethodNotExists = 2032,
        PaymentFailed = 2033,
        PaymentSuccess = 2034,
        PaymentNotExists = 2035,
        InvalidSignature = 2036,
        PaymentNotFoundByUserId = 2037,
        CategorySameName = 2038,
        TransactionSuspectedOfFraud = 2039,
        AccountNotRegisteredForInternetBanking = 2040,
        CardAccountAuthenticationFailedMoreThan3Times = 2041,
        PaymentTimeout = 2042,
        CardAccountIsLocked = 2043,
        IncorrectTransactionAuthenticationPassword = 2044,
        TransactionCanceledByCustomer = 2045,
        InsufficientAccountBalance = 2046,
        TransactionLimitExceeded = 2047,
        BankIsUnderMaintenance = 2048,
        IncorrectPaymentPasswordExceeded = 2049,
        UndefinedError = 2050,
        UserSubscriptionExpired = 2051
    }
}
