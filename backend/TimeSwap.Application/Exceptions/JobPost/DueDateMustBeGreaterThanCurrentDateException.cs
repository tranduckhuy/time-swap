using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class DueDateMustBeGreaterThanCurrentDateException : AppException
    {
        public DueDateMustBeGreaterThanCurrentDateException() 
            : base(StatusCode.DueDateMustBeGreaterThanCurrentDate)
        {
        }
    }
}
