using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class Payment : AuditableEntityBase<Guid>
    {
        public Payment() : base() 
        {
            Id = Guid.NewGuid();
            TransactionId = Guid.NewGuid();
        }

        public Guid UserId { get; set; }

        public string PaymentContent { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }

        public Guid TransactionId { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public DateTime ExpiryDate { get; set; }

        // Navigation properties
        public virtual UserProfile User { get; set; } = null!;
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
        public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = [];

    }
}
