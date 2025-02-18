using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class PaymentFailedException : AppException
    {
        public PaymentFailedException() : base(StatusCode.PaymentFailed)
        {
        }
    }
}
