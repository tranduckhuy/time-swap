namespace TimeSwap.Domain.Entities
{
    public class Review : AuditableEntityBase<Guid>
    {
        public Guid JobPostId { get; set; }

        public Guid ReviewerId { get; set; }

        public Guid RevieweeId { get; set; }

        public string? Comment { get; set; }

        // Navigation properties
        public virtual JobPost JobPost { get; set; } = null!;
        public virtual ICollection<ReviewCriteria> ReviewCriterias { get; set; } = [];
    }
}
