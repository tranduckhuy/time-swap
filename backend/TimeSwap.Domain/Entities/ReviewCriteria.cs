using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class ReviewCriteria : EntityBase<Guid>
    {
        public Guid ReviewId { get; set; }

        public ReviewCriteriaType CriteriaName { get; set; }

        public int Rating { get; set; }
    }
}
