using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class AssignJobToOwnerException : AppException
    {
        public AssignJobToOwnerException() : base(StatusCode.AssignJobToOwner)
        {
        }
    }
}
