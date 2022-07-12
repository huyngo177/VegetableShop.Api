using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Models.Sale
{
    public class Items
    {
        public int Quantity { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
