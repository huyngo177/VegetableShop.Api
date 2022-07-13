namespace VegetableShop.Api.Dto.Orders
{
    public class CartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
