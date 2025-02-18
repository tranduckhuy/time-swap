using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class InsufficientAccountBalanceException : AppException
    {
        public InsufficientAccountBalanceException() : base(StatusCode.InsufficientAccountBalance)
        {
        }
    }
}
