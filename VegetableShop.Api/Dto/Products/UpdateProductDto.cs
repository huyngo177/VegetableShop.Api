namespace VegetableShop.Api.Dto.Products
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public decimal Price { set; get; }
        public int Stock { set; get; }
        public DateTime DateUpdated { set; get; } = DateTime.UtcNow;
        public IFormFile? Image { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
    }
}
