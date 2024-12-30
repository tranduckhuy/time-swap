﻿using TimeSwap.Shared.Constants;

namespace TimeSwap.Domain.Entities
{
    public class UserProfile : AuditableEntityBase<Guid>
    {
        public SubscriptionPlan CurrentSubscription { get; set; } = SubscriptionPlan.Basic;

        public DateTime SubscriptionExpiryDate { get; set; }
        
        public decimal Balance { get; set; }
        
        public string Description { get; set; } = string.Empty;
        
        public string AvatarUrl { get; set; } = string.Empty;

        public IList<string> LocationIds { get; set; } = [];


        // Navigation properties
        public virtual ICollection<JobPost> JobPosts { get; set; } = [];
        public virtual ICollection<JobApplicant> AppliedJobs { get; set; } = [];
        public virtual ICollection<Payment> Payments { get; set; } = [];
        public virtual ICollection<SubscriptionHistory> SubscriptionHistories { get; set; } = [];
        public virtual ICollection<Conversation> Conversations { get; set; } = [];
        public virtual ICollection<ConversationReply> ConversationReplies { get; set; } = [];

    }
}
