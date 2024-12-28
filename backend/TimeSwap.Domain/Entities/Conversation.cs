namespace TimeSwap.Domain.Entities
{
    public class Conversation : EntityBase<Guid>
    {
        public Guid UserOneId { get; set; }

        public Guid UserTwoId { get; set; }

        public DateTime LastMessageDate { get; set; }

        // Navigation properties
        public virtual ICollection<ConversationReply> ConversationReplies { get; set; } = [];
    }
}
