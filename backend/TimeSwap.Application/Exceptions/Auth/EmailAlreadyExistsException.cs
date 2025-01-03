using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class EmailAlreadyExistsException : AppException
    {
        public EmailAlreadyExistsException() : base(StatusCode.EmailAlreadyExists) { }
    }
}
