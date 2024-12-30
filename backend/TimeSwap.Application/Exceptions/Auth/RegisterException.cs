using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class RegisterException : AuthException
    {
        public RegisterException() : base(StatusCode.RegisterFailed) { }
    }
}
