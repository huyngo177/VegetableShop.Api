using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Models.Role
{
    public class UpdateRoleRequest
    {
        [Required(ErrorMessage = "Please enter role name")]
        [StringLength(20, ErrorMessage = "Role name must greater than 4 characters and less than 20 characters", MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Role name must greater than 2 characters and less than 255 characters", MinimumLength = 2)]
        public string? Description { get; set; }
    }
}
