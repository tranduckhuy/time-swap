using Microsoft.AspNetCore.WebUtilities;
using TimeSwap.Application.Authentication.Dtos.Requests;
using TimeSwap.Application.Email;
using TimeSwap.Infrastructure.Identity;

namespace TimeSwap.Infrastructure.Email
{
    public static class MailMessageHelper
    {
        public static Message CreateMessage(ApplicationUser user, string token, string clientUrl, string subject, string content)
        {

            var param = new Dictionary<string, string>
            {
                { "token", token },
                { "email", user.Email! }
            };

            var callbackUrl = QueryHelpers.AddQueryString(clientUrl, param!);

            var message = new Message(
                [(user.UserName!, user.Email!)],
                subject,
                $"Please {content} by <a href='{callbackUrl}'>clicking here</a>.",
                null
            );

            return message;
        }

        public static void CreateLockAccountMessage(LockUnlockAccountRequestDto request,
            ApplicationUser user, out string userName, out string emailSubject, out string emailBody)
        {
            var reason = request.Reason ?? "suspicious activity";
            userName = user.FirstName + " " + user.LastName;
            emailSubject = "Account Locked";
            emailBody = $"Hello {userName},\n\n" +
                            $"Your account has been locked at {DateTime.UtcNow}. " +
                            $"The reason for this action is: {reason}. \n\n" +
                            "If you believe this is an error or need further assistance, please contact our support team.";
        }
    }
}
