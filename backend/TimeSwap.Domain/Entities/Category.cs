namespace TimeSwap.Domain.Entities
{
    public class Category : EntityBase<int>
    {
        public string CategoryName { get; set; } = string.Empty;

        public int IndustryId { get; set; }

        // Navigation properties
        public virtual Industry Industry { get; set; } = null!;
    }
}
