using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.PaymentMethods
{
    public class PaymentMethodNotExistsException : AppException
    {
        public PaymentMethodNotExistsException() : base(StatusCode.PaymentMethodNotExists) { }
    }
}
