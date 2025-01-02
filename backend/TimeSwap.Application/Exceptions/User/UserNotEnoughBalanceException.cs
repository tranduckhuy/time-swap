using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.User
{
    public class UserNotEnoughBalanceException : AppException
    {
        public UserNotEnoughBalanceException() : base(StatusCode.UserNotEnoughBalance) { }
    }
}
