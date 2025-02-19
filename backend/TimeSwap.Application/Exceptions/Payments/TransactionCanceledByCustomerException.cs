using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class TransactionCanceledByCustomerException : AppException
    {
        public TransactionCanceledByCustomerException() : base(StatusCode.TransactionCanceledByCustomer)
        {
        }
    }
}
