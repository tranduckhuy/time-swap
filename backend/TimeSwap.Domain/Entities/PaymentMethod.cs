using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class PaymentMethod : EntityBase<int>
    {
        public PaymentMethodType PaymentMethodType { get; set; }

        public string MethodDetailName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
