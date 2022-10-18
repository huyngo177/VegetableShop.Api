using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Mvc.Models.User
{
    public class UpdatePasswordRequest
    {
        [Required(ErrorMessage = "Please enter current password")]
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter new password")]
        [StringLength(20, ErrorMessage = "Password must greater 6 characters and less than 20 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Confirm password must match new password")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
