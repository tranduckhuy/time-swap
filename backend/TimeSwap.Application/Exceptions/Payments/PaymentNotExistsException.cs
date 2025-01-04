using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class PaymentNotExistsException : AppException
    {
        public PaymentNotExistsException() : base(StatusCode.PaymentNotExists) { }
    }
}
