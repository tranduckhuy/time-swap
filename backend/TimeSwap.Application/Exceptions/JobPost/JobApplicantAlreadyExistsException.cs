using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class JobApplicantAlreadyExistsException : AppException
    {
        public JobApplicantAlreadyExistsException() : base(StatusCode.JobApplicantAlreadyExists)
        {
        }
    }
}
