using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class SubscriptionHistory : EntityBase<Guid>
    {
        public Guid UserId { get; set; }

        public SubscriptionPlan Subscription { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
