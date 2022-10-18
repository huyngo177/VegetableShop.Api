using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Mvc.Models.User
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Please enter username")]
        [StringLength(20, ErrorMessage = "Username must greater than 2 characters and less than 20 characters", MinimumLength = 2)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Email invalid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(20, ErrorMessage = "Password must greater 6 characters and less than 20 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password must match password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(20, ErrorMessage = "Firstname must greater than 2 characters and less than 20 characters", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(20, ErrorMessage = "Lastname must greater than 2 characters and less than 20 characters", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})", ErrorMessage = "PhoneNumber invalid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        [StringLength(255, ErrorMessage = "Address must greater than 2 characters and less than 255 characters")]
        public string Address { get; set; }
    }
}
