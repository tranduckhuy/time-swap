using TimeSwap.Domain.Exceptions;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Location
{
    public class InvalidWardInCityException : AppException
    {
        public InvalidWardInCityException() : base(StatusCode.InvalidWardInCity) { }
    }
}
