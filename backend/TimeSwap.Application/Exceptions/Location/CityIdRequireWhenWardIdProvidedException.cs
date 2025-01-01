using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Location
{
    public class CityIdRequireWhenWardIdProvidedException : AppException
    {
        public CityIdRequireWhenWardIdProvidedException() : base(StatusCode.CityIdRequireWhenWardIdProvidedException) { }
    }
}
