using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class UserAccountLockedException : AuthException
    {
        public UserAccountLockedException() : base(StatusCode.UserAccountLocked)
        {
        }
    }
}
