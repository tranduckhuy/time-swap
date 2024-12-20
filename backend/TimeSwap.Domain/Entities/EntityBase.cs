namespace TimeSwap.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }

        // Audit properties
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime LastModifiedDate { get; set; }
    }
}
