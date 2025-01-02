using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class DueDateMustBeGreaterThanStartDateException : AppException
    {
        public DueDateMustBeGreaterThanStartDateException() 
            : base(StatusCode.DueDateMustBeGreaterThanStartDate)
        {
        }
    }
}
