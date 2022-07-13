using VegetableShop.Api.Data.Entities;

namespace VegetableShop.Api.Dto.Orders
{
    public class CreateOrderDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { set; get; } = DateTime.Now;
        public List<ItemDto> Items { get; set; }
    }
}
