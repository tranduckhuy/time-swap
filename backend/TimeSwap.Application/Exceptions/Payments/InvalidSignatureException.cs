using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class InvalidSignatureException : AppException
    {
        public InvalidSignatureException() : base(StatusCode.InvalidSignature) { }
    }
}
