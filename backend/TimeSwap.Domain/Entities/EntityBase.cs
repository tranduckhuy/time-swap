using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Domain.Entities
{
    public abstract class EntityBase<T>
    {
        [Key]
        public T Id { get; set; } = default!;
    }
}
