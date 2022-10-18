using System.ComponentModel;

namespace VegetableShop.Mvc.Models.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [DisplayName("Firstname")]
        public string FirstName { get; set; }

        [DisplayName("Lastname")]
        public string LastName { get; set; }

        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public IList<string> Roles { get; set; }
    }
}
