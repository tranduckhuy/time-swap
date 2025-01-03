using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class TransactionLog : EntityBase<Guid>
    {
        public Guid TransactionId { get; set; }
        
        public TransactionEvent TransactionEvent { get; set; }

        public Guid PaymentId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual Payment Payment { get; set; } = null!;
    }
}
