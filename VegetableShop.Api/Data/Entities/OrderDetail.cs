namespace VegetableShop.Api.Data.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal SupTotal { set; get; }
        public int OrderId { set; get; }
        public virtual Order Order { get; set; }
        public virtual Product Products { get; set; }
        public int ProductId { set; get; }
    }
}
