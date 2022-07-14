using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Sale;

namespace VegetableShop.Mvc.ApiClient.Carts
{
    public interface ICartApiClient
    {
        Task<List<CartItemViewModel>> AddToCart(int id, int quantity);
        List<CartItemViewModel> GetListItem();
        IEnumerable<CartItemViewModel> UpdateCart(int id, int quantity);
        Task<CreateResponse> Checkout(UserInfoRequest userInfoRequest);
        CheckoutViewModel GetCheckoutViewModel();
        void RemoveCart();
        List<CartItemViewModel> RemoveItemInCart(int id);
    }
}
