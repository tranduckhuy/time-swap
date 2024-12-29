using TimeSwap.Shared.Constants;

namespace TimeSwap.Application.Exceptions.Auth
{
    public class ModelInvalidException : Exception
    {
        public static StatusCode StatusCode { get; } = StatusCode.ModelInvalid;

        public ModelInvalidException() : base(ResponseMessages.GetMessage(StatusCode))
        {
        }
    }
}
