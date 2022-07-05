using System.ComponentModel.DataAnnotations.Schema;

namespace VegetableShop.Api.Data.Entities
{
    public class Cart : BaseEntity
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public virtual Product Product { get; set; }
        public DateTime DateCreated { get; set; }
        public int AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
