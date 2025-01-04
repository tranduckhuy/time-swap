using MediatR;

namespace TimeSwap.Application.Payments.Commands
{
    public class CreatePaymentCommand : IRequest<string>
    {
        public Guid UserId { get; set; }

        public string? PaymentContent { get; set; }

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }

        public string IpAddress { get; set; } = string.Empty;
    }
}
