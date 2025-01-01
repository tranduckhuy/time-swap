using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Industries
{
    public class IndustrySameNameException : AppException
    {
        public IndustrySameNameException() : base(StatusCode.IndustrySameName) { }
    }
}
