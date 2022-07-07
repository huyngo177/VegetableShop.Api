using System.ComponentModel;

namespace VegetableShop.Mvc.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { set; get; }
        public int Stock { set; get; }
        [DisplayName("Created Date")]
        public DateTime DateCreated { set; get; }
        [DisplayName("Updated Date")]
        public DateTime? DateUpdated { set; get; }
        [DisplayName("Image")]
        public string? ImagePath { get; set; }
        [DisplayName("Category Name")]
        public string? CategoryName { get; set; }
        public string Status { get; set; }
    }
}
