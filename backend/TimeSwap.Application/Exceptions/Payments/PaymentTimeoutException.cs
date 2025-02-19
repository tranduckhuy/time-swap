using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class PaymentTimeoutException : AppException
    {
        public PaymentTimeoutException() : base(StatusCode.PaymentTimeout)
        {
        }
    }
}
