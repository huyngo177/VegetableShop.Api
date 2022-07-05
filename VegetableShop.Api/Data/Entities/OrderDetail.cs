namespace VegetableShop.Api.Data.Entities
{
    public class OrderDetail : BaseEntity
    {
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public int OrderId { set; get; }
        public virtual Order Order { get; set; }
        public int ProductId { set; get; }
        public virtual Product Product { get; set; }
    }
}
