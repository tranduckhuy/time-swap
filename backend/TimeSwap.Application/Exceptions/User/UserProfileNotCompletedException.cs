using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.User
{
    public class UserProfileNotCompletedException : AppException
    {
        public UserProfileNotCompletedException() : base(StatusCode.UserProfileNotCompleted)
        {
        }
    }
}
