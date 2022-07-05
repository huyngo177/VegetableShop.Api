using Microsoft.AspNetCore.Identity;

namespace VegetableShop.Api.Data.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
