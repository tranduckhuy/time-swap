﻿namespace TimeSwap.Shared.Constants
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
            { StatusCode.ModelInvalid, "Model is invalid." },
            { StatusCode.EmailAlreadyExists, "Email already exists." },
            { StatusCode.RegisterFailed, "Register failed." },
            { StatusCode.UserNotExists, "User does not exist in the system." },
            { StatusCode.UserNotConfirmed, "User is not confirmed." },
            { StatusCode.InvalidCredentials, "Invalid credentials. Please try again." },
            { StatusCode.UserAuthenticationFailed, "User authentication failed." },
            { StatusCode.ConfirmEmailTokenInvalidOrExpired, "Confirm email token is invalid or expired." },
            { StatusCode.UserAlreadyConfirmed, "User is already confirmed." },
            { StatusCode.InvalidToken, "Invalid token." },
            { StatusCode.ProvidedInformationIsInValid, "Provided information is invalid." },
            { StatusCode.TokenIsBlacklisted, "Token is blacklisted." },
            { StatusCode.RequestProcessingFailed, "The request processing has failed." },
            { StatusCode.IndustryNotFound, "Industry with Id does not exist." },
            { StatusCode.IndustrySameName, "Industry with the same name already exists." },
            { StatusCode.CategoryNotFound, "Category with Id does not exist." },
            { StatusCode.CategoryNotFoundByIndustryId, "No categories found for the given industry." }
        };

        public static string GetMessage(StatusCode code) => _messages[code];
    }
}
