using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class JobPostAlreadyAssignedException : AppException
    {
        public JobPostAlreadyAssignedException() : base(StatusCode.JobPostAlreadyAssigned)
        {
        }
    }
}
