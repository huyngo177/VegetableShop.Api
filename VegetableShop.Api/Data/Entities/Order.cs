namespace VegetableShop.Api.Data.Entities
{
    public class Order : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { set; get; } = DateTime.Now;
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public int AppUserId { set; get; }
        public virtual AppUser AppUser { get; set; }
    }
}

