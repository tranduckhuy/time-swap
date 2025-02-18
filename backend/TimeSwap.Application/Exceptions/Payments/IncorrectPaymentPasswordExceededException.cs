using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class IncorrectPaymentPasswordExceededException : AppException
    {
        public IncorrectPaymentPasswordExceededException() : base(StatusCode.IncorrectPaymentPasswordExceeded)
        {
        }
    }
}
