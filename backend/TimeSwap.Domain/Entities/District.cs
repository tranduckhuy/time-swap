using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Domain.Entities
{
    public class District : EntityBase<string>
    {
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(30)]
        public string CityId { get; set; } = string.Empty;

        // Navigation properties
        public virtual City City { get; set; } = null!;
        public virtual ICollection<Ward> Wards { get; set; } = [];
    }
}
