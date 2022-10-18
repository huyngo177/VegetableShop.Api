using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Mvc.Models.User
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(20, ErrorMessage = "Firstname must less 20 characters and greater than 2 characters", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(20, ErrorMessage = "Lastname must less 20 characters and greater than 2 characters", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})", ErrorMessage = "PhoneNumber invalid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        [StringLength(100, ErrorMessage = "Address must less than 100 characters")]
        public string Address { get; set; }
    }
}
