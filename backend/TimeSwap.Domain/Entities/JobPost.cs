﻿namespace TimeSwap.Domain.Entities
{
    public class JobPost : AuditableEntityBase<Guid>
    {
        public JobPost() : base()
        {
            Id = Guid.NewGuid();
        }

        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Responsibilities { get; set; } = string.Empty;

        public decimal Fee { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public Guid? AssignedTo { get; set; }

        public bool IsOwnerCompleted { get; set; }

        public bool IsAssigneeCompleted { get; set; }

        public int CategoryId { get; set; }

        public int IndustryId { get; set; }

        public string? WardId { get; set; }

        public string? CityId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual Industry Industry { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual Ward Ward { get; set; } = null!;
        public virtual City City { get; set; } = null!;
        public virtual ICollection<JobApplicant> JobApplicants { get; set; } = [];
        public virtual ICollection<Review> Reviews { get; set; } = [];
    }
}
