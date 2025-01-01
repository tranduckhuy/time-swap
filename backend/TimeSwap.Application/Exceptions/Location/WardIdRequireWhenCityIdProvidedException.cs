using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Location
{
    public class WardIdRequireWhenCityIdProvidedException : AppException
    {
        public WardIdRequireWhenCityIdProvidedException() : base(StatusCode.WardIdRequireWhenCityIdProvidedException) { }
    }
}
