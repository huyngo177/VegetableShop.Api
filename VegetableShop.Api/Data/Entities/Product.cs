﻿namespace VegetableShop.Api.Data.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public DateTime DateCreated { set; get; }
        public virtual List<OrderDetail> OrderDetail { get; set; }
        public string? ImagePath { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Status { get; set; } = "Available";
        public string? Description { get; set; }
    }
}
