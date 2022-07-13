using System.ComponentModel.DataAnnotations;
using VegetableShop.Api.Dto.Orders;

namespace VegetableShop.Mvc.Models.Orders
{
    public class UpdateOrderRequest
    {
        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression("(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})", ErrorMessage = "PhoneNumber invalid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        [StringLength(255, ErrorMessage = "Address must greater than 2 characters and less than 255 characters")]
        public string Address { get; set; }
        public DateTime OrderDate { set; get; }
        public List<ItemDto> Items { get; set; }
    }
}
