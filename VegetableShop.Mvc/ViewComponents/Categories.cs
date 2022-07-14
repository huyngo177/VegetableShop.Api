using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Categories;

namespace VegetableShop.Mvc.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public Categories(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryApiClient.SelectAll();
            return View(categories);
        }
    }
}
