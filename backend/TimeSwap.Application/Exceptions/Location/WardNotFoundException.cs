using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Location
{
    public class WardNotFoundException : AppException
    {
        public WardNotFoundException(IEnumerable<string>? errors = null) 
            : base(StatusCode.WardNotFound, errors)
        {
        }
    }
}
