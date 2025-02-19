using System.Diagnostics.CodeAnalysis;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class UserProfile : AuditableEntityBase<Guid>
    {
        public SubscriptionPlan CurrentSubscription { get; set; } = SubscriptionPlan.Basic;

        public DateTime? SubscriptionExpiryDate { get; set; }
        
        public decimal Balance { get; set; }
        
        public string? Description { get; set; }

        public string AvatarUrl { get; set; } = AppConstant.DEFAULT_AVATAR;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? CityId { get; set; }

        public string? WardId { get; set; }

        public IList<string>? EducationHistory { get; set; }

        [AllowNull]
        public int? MajorCategoryId { get; set; }

        [AllowNull]
        public int? MajorIndustryId { get; set; }

        // Navigation properties
        public virtual Category MajorCategory { get; set; } = null!;
        public virtual Industry MajorIndustry { get; set; } = null!;
        public virtual City City { get; set; } = null!;
        public virtual Ward Ward { get; set; } = null!;
        public virtual ICollection<JobPost> JobPosts { get; set; } = [];
        public virtual ICollection<JobApplicant> AppliedJobs { get; set; } = [];
        public virtual ICollection<Payment> Payments { get; set; } = [];
        public virtual ICollection<SubscriptionHistory> SubscriptionHistories { get; set; } = [];
        public virtual ICollection<Conversation> Conversations { get; set; } = [];
        public virtual ICollection<ConversationReply> ConversationReplies { get; set; } = [];

    }
}
