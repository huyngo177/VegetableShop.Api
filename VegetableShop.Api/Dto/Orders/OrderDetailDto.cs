namespace VegetableShop.Api.Dto.Orders
{
    public class OrderDetailDto
    {
        public decimal Price { set; get; }
        public int Quantity { set; get; }
        public string ProductName { set; get; }
    }
}
