using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class UserNotAppliedToJobPostException : AppException
    {
        public UserNotAppliedToJobPostException() : base(StatusCode.UserNotAppliedToJobPost)
        {
        }
    }
}
