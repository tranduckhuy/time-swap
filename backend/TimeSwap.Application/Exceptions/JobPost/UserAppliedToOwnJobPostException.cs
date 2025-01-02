using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class UserAppliedToOwnJobPostException : AppException
    {
        public UserAppliedToOwnJobPostException() : base(StatusCode.UserAppliedToOwnJobPost)
        {
        }
    }
}
