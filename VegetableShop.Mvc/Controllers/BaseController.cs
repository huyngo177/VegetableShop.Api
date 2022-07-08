using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VegetableShop.Mvc.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
