namespace TimeSwap.Domain.Entities
{
    public class Category : EntityBase<int>
    {
        public string CategoryName { get; set; } = string.Empty;
    }
}
