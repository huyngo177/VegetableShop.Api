using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.Models.Products
{
    public class ProductDetailViewModel
    {
        public CategoryViewModel Category { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
