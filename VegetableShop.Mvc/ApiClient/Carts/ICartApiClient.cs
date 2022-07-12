using VegetableShop.Mvc.Models.Sale;

namespace VegetableShop.Mvc.ApiClient.Carts
{
    public interface ICartApiClient
    {
        Task<List<CartItemViewModel>> AddToCart(int id, int quantity);
        List<CartItemViewModel> GetListItem();
        IEnumerable<CartItemViewModel> UpdateCart(int id, int quantity);
        Task<bool> Checkout(CheckoutViewModel checkoutViewModel);
        CheckoutViewModel GetCheckoutViewModel();
        bool RemoveCart();
    }
}
