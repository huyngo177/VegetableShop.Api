using Microsoft.AspNetCore.Identity;

namespace VegetableShop.Api.Data.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual List<Order> Orders { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsLocked { get; set; }
    }
}
