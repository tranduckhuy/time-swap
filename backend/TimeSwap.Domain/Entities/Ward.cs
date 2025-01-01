using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Domain.Entities
{
    public class Ward : EntityBase<string>
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(30)]
        public string DistrictId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Level { get; set; } = string.Empty;

        [MaxLength(255)]
        public string FullLocation { get; set; } = string.Empty;

        // Navigation properties
        public virtual District District { get; set; } = null!;
    }
}
