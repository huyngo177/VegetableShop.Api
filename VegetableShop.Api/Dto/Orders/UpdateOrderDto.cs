namespace VegetableShop.Api.Dto.Orders
{
    public class UpdateOrderDto
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { set; get; }
        public List<ItemDto> Items { get; set; }
    }
}
