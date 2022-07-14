using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Page;

namespace VegetableShop.Mvc.ViewComponents
{
    public class Page : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
