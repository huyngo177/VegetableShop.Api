using VegetableShop.Api.Data.Entities;

namespace VegetableShop.Api.Dto.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
