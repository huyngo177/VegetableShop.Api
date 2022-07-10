using System.ComponentModel;
using VegetableShop.Api.Data.Entities;
namespace VegetableShop.Mvc.Models.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Products List")]
        public List<Product> Products { get; set; }
    }
}
