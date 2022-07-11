namespace VegetableShop.Api.Dto.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { set; get; }
        public int Stock { set; get; }
        public DateTime DateCreated { set; get; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
    }
}
