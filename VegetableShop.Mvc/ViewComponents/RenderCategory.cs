using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Categories;

namespace VegetableShop.Mvc.ViewComponents
{
    public class RenderCategory : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public RenderCategory(ICategoryApiClient categoryApiClient)
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
