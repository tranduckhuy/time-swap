namespace TimeSwap.Domain.Entities
{
    public class ConversationReply : EntityBase<Guid>
    {
        public string Message { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public DateTime SentAt { get; set; }

        public Guid ConversationId { get; set; }

        // Navigation properties

    }
}
