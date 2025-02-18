using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class TransactionLimitExceededException : AppException
    {
        public TransactionLimitExceededException() : base(StatusCode.TransactionLimitExceeded)
        {
        }
    }
}
