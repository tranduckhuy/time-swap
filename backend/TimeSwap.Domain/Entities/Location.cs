namespace TimeSwap.Domain.Entities
{
    public class Location : AuditableEntityBase<Guid>
    {
        public string? Street { get; set; }

        public string Ward { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;
    }
}
