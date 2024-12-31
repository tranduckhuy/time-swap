using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Domain.Entities
{
    public class City : EntityBase<string>
    {
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Img { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<District> Districts { get; set; } = [];
    }
}
