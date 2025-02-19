using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.User
{
    public class UserSubscriptionExpiredException : AuthException
    {
        public UserSubscriptionExpiredException() : base(StatusCode.UserSubscriptionExpired) { }
    }
}
