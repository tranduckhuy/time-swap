namespace TimeSwap.Domain.Entities
{
    public class JobApplicant : EntityBase<Guid>
    {
        public JobApplicant() : base()
        {
            Id = Guid.NewGuid();
        }

        public Guid JobPostId { get; set; }

        public Guid UserAppliedId { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual JobPost JobPost { get; set; } = null!;
        public virtual UserProfile UserApplied { get; set; } = null!;
    }
}
