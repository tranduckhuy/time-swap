using MediatR;

namespace TimeSwap.Application.Payments.Commands
{
    public class PayOSReturnCommand : IRequest<string>
    {
        public string status { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string orderCode { get; set; } = string.Empty;
        public bool cancel { get; set; }
    }
}
