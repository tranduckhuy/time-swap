using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class InvalidTokenException : AuthException
    {
        public InvalidTokenException(IEnumerable<string>? errors = null) : base(StatusCode.InvalidToken, errors) { }
    }
}
