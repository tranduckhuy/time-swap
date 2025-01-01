using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Industries
{
    public class IndustryNotFoundException : AppException
    {
        public IndustryNotFoundException() : base(StatusCode.IndustryNotFound) { }
    }
}
