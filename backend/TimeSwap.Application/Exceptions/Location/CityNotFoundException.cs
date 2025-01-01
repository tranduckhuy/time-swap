using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Location
{
    public class CityNotFoundException : AppException
    {
        public CityNotFoundException(IEnumerable<string>? errors = null) : base(StatusCode.CityNotFound, errors)
        {
        }
    }
}
