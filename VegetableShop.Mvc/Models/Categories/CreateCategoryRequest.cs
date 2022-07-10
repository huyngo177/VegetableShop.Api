using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Mvc.Models.Categories
{
    public class CreateCategoryRequest
    {
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Please enter category name")]
        [StringLength(20, ErrorMessage = "Category name must greater than 2 characters and less than 20 characters", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
