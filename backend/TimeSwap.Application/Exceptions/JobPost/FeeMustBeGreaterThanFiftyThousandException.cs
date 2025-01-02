using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.JobPost
{
    public class FeeMustBeGreaterThanFiftyThousandException : AppException
    {
        public FeeMustBeGreaterThanFiftyThousandException() : base(StatusCode.FeeMustBeGreaterThanFiftyThousand)
        {
        }
    }
}
