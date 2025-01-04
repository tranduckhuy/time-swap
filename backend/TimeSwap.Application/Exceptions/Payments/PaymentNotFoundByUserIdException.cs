using TimeSwap.Shared.Constants;
using TimeSwap.Domain.Exceptions;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class PaymentNotFoundByUserIdException : AppException
    {
        public PaymentNotFoundByUserIdException() : base(StatusCode.PaymentNotFoundByUserIdException) { }
    }
}
