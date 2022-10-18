using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VegetableShop.Mvc.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }
    }
}
