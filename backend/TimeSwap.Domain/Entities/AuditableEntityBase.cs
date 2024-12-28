namespace TimeSwap.Domain.Entities
{
    public abstract class AuditableEntityBase<T> : EntityBase<T>
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
    }
}
