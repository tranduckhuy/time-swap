﻿namespace TimeSwap.Domain.Entities
{
    public class Industry : EntityBase<int>
    {
        public string IndustryName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Category> Categories { get; set; } = [];
    }
}
