using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class TransactionSuspectedOfFraudException : AppException
    {
        public TransactionSuspectedOfFraudException() : base(StatusCode.TransactionSuspectedOfFraud) { }
    }
}
