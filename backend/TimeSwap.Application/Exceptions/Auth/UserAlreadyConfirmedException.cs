using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class UserAlreadyConfirmedException : AppException
    {
        public UserAlreadyConfirmedException() : base(StatusCode.UserAlreadyConfirmed) { }
    }
}
