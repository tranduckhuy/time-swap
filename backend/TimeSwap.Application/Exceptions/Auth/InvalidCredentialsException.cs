using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class InvalidCredentialsException : AuthException
    {
        public InvalidCredentialsException() : base(StatusCode.InvalidCredentials) { }
    }
}
