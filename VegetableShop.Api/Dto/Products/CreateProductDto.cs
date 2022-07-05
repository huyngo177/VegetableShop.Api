namespace VegetableShop.Api.Dto.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { set; get; }
        public int Stock { set; get; }
        public DateTime DateCreated { set; get; }
        public IFormFile? Image { get; set; }
        public string? CategoryName { get; set; }
    }
}
