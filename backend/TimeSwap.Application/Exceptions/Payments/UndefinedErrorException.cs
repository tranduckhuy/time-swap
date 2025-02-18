using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class UndefinedErrorException : AppException
    {
        public UndefinedErrorException() : base(StatusCode.UndefinedError)
        {
        }
    }
}
