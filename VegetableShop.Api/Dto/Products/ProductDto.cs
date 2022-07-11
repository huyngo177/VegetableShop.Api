namespace VegetableShop.Api.Dto.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { set; get; }
        public int Stock { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime? DateUpdated { set; get; }
        public string? ImagePath { get; set; }
        public string? CategoryName { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
