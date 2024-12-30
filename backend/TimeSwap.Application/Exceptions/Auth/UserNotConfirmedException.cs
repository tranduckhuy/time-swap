using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class UserNotConfirmedException : AuthException
    {
        public UserNotConfirmedException() : base(StatusCode.UserNotConfirmed) { }
    }
}
