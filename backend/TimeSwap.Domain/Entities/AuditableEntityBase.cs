namespace TimeSwap.Domain.Entities
{
    public abstract class AuditableEntityBase<T> : EntityBase<T>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }
    }
}
