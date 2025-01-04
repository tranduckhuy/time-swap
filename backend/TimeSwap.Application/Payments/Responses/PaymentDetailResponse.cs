namespace TimeSwap.Application.Payments.Responses
{
    public class PaymentDetailResponse
    {
        public Guid PaymentId { get; set; }

        public string PaymentContent { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string PaymentMethodType { get; set; } = string.Empty;

        public string MethodDetailName { get; set; } = string.Empty;
    }
}
