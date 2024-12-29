using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class UserNotExistsException : AppException
    {
        public UserNotExistsException() : base(StatusCode.UserNotExists) { }
    }
}
