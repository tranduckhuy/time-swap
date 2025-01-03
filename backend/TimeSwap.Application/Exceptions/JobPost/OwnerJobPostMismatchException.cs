using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class OwnerJobPostMismatchException : AppException
    {
        public OwnerJobPostMismatchException() : base(StatusCode.OwnerJobPostMismatch)
        {
        }
    }
}
