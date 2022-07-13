using VegetableShop.Api.Data.Entities;

namespace VegetableShop.Api.Dto.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { set; get; }
        public OrderDetail OrderDetails { get; set; }
    }
}
