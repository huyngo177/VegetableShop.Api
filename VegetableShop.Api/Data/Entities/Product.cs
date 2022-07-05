namespace VegetableShop.Api.Data.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price
        {
            get
            {
                return Price;
            }
            set
            {
                if (value < 0)
                {
                    Price = 0;
                }
                else Price = value;
            }
        }
        public int Stock
        {
            get
            {
                return Stock;
            }
            set
            {
                if (value < 0)
                {
                    Stock = 0;
                }
                else Stock = value;
            }
        }
        public DateTime DateCreated { set; get; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual List<Cart> Carts { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
