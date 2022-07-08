using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Mvc.Models.Products
{
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Please enter product name")]
        [StringLength(20, ErrorMessage = "Product name must greater than 2 characters and less than 20 characters", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter price")]
        [Range(typeof(decimal), "0", "999999", ErrorMessage = "{0} must be a number between {1} and {2}")]
        public decimal Price { set; get; }
        [Required(ErrorMessage = "Please enter price")]
        [Range(typeof(int), "0", "9999", ErrorMessage = "{0} must be a number between {1} and {2}")]
        public int Stock { set; get; }
        public DateTime DateUpdated { set; get; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
    }
}
