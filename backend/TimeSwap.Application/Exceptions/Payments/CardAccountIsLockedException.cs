using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Payments
{
    public class CardAccountIsLockedException : AppException
    {
        public CardAccountIsLockedException() : base(StatusCode.CardAccountIsLocked)
        {
        }
    }
}
