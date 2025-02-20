namespace TimeSwap.Shared.Constants
{
    public static class ResponseMessages
    {
        private static readonly Dictionary<StatusCode, string> _messages = new Dictionary<StatusCode, string>
        {
            // Success messages
            { StatusCode.RequestProcessedSuccessfully, "Request processed successfully." },
            { StatusCode.ConfirmationEmailSent, "Confirmation email sent." },
            { StatusCode.LoginSuccessful, "Login successful." },
            { StatusCode.ResetPasswordEmailSent, "Reset password email sent." },
            { StatusCode.PasswordResetSuccessful, "Password reset successful." },

            // Error messages
            { StatusCode.ModelInvalid, "Model is invalid. Please check the request body." },
            { StatusCode.EmailAlreadyExists, "Email already exists. Please try with another email." },
            { StatusCode.RegisterFailed, "Register failed. Please try again." },
            { StatusCode.UserNotExists, "User does not exist in the system." },
            { StatusCode.UserNotConfirmed, "User is not confirmed. Please confirm your email." },
            { StatusCode.InvalidCredentials, "Invalid credentials. Please try again." },
            { StatusCode.UserAuthenticationFailed, "User authentication failed. Please try again." },
            { StatusCode.ConfirmEmailTokenInvalidOrExpired, "Confirm email token is invalid or expired." },
            { StatusCode.UserAlreadyConfirmed, "User is already confirmed." },
            { StatusCode.InvalidToken, "Invalid token. Please try again." },
            { StatusCode.ProvidedInformationIsInValid, "Provided information is invalid." },
            { StatusCode.TokenIsBlacklisted, "Token is blacklisted. Please login to get a new token." },
            { StatusCode.RequestProcessingFailed, "The request processing has failed." },
            { StatusCode.IndustryNotFound, "Industry with Id does not exist." },
            { StatusCode.IndustrySameName, "Industry with the same name already exists." },
            { StatusCode.CategoryNotFound, "Category with Id does not exist." },
            { StatusCode.WardNotFound, "Ward with Id does not exist." },
            { StatusCode.CityNotFound, "City with Id does not exist." },
            { StatusCode.JobPostNotFound, "Job post with Id does not exist." },
            { StatusCode.InvalidWardInCity, "Ward does not belong to the city." },
            { StatusCode.WardIdRequireWhenCityIdProvided, "WardId is required when CityId is provided in the request." },
            { StatusCode.CityIdRequireWhenWardIdProvided, "CityId is required when WardId is provided in the request." },
            { StatusCode.InvalidCategoryInIndustry, "Category does not belong to the industry." },
            { StatusCode.UserNotEnoughBalance, "User does not have enough balance to create a job post." },
            { StatusCode.DueDateMustBeGreaterThanStartDate, "Due date must be greater than the start date." },
            { StatusCode.DueDateMustBeGreaterThanCurrentDate, "Due date must be greater than the current date." },
            { StatusCode.FeeMustBeGreaterThanFiftyThousand, "Fee must be greater than 50,000 VND." },
            { StatusCode.UserNotAppliedToJobPost, "User has not applied to the job post." },
            { StatusCode.JobPostAlreadyAssigned, "Job post has already been assigned to another user." },
            { StatusCode.AssignJobToOwner, "Cannot assign job to the owner. Please assign to another user." },
            { StatusCode.OwnerJobPostMismatch, "The owner of the job post does not match the user." },
            { StatusCode.PaymentMethodNotExists, "Payment method does not exist." },
            { StatusCode.PaymentFailed, "Payment failed. Please try again." },
            { StatusCode.PaymentSuccess, "Payment successful." },
            { StatusCode.PaymentNotExists, "Payment does not exist." },
            { StatusCode.InvalidSignature, "Invalid signature." },
            { StatusCode.PaymentNotFoundByUserId, "No payments found for the user." },
            { StatusCode.UserAppliedToOwnJobPost, "You cannot apply to your own job post. Please try with another job post." },
            { StatusCode.CategorySameName, "Category with the same name already exists." },
            { StatusCode.TransactionSuspectedOfFraud, "Transaction suspected of fraud." },
            { StatusCode.AccountNotRegisteredForInternetBanking, "Account not registered for Internet Banking." },
            { StatusCode.CardAccountAuthenticationFailedMoreThan3Times, "Card account authentication failed more than 3 times." },
            { StatusCode.PaymentTimeout, "Payment timeout." },
            { StatusCode.CardAccountIsLocked, "Card account is locked." },
            { StatusCode.IncorrectTransactionAuthenticationPassword, "Incorrect transaction authentication password." },
            { StatusCode.TransactionCanceledByCustomer, "Transaction canceled by customer." },
            { StatusCode.InsufficientAccountBalance, "Insufficient account balance." },
            { StatusCode.TransactionLimitExceeded, "Transaction limit exceeded." },
            { StatusCode.BankIsUnderMaintenance, "Bank maintenance in progress." },
            { StatusCode.IncorrectPaymentPasswordExceeded, "Payment password entered incorrectly too many times." },
            { StatusCode.UndefinedError, "Undefined error." },
            { StatusCode.UserSubscriptionExpired, "Your subscription has expired. Please renew your subscription." }
        };

        public static string GetMessage(StatusCode code) => _messages[code];
    }
}
