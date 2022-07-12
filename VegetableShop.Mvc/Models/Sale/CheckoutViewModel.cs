namespace VegetableShop.Mvc.Models.Sale
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public CheckOutRequest CheckOutRequest { get; set; }
    }
}
