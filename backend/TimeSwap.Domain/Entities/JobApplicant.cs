namespace TimeSwap.Domain.Entities
{
    public class JobApplicant : EntityBase<Guid>
    {
        public Guid JobPostId { get; set; }

        public Guid UserAppliedId { get; set; }

        public DateTime AppliedAt { get; set; }

        // Navigation properties
        public virtual JobPost JobPost { get; set; } = null!;
    }
}
