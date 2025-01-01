using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class JobPostNotFoundException : AppException
    {
        public JobPostNotFoundException() : base(StatusCode.JobPostNotFound) { }
    }
}
