using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.User;
using VegetableShop.Mvc.Models.Page;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IMapper _mapper;
        public UserController(IUserApiClient userApiClient, IMapper mapper)
        {
            _userApiClient = userApiClient;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, SortOrder sortOrder = 0, int pageIndex = 1, int pageSize = 5, string sortExpression = "id")
        {
            SortItems sortModel = new SortItems();
            sortModel.ApplySort(sortExpression);

            ViewBag.Keyword = keyword;
            var users = await _userApiClient.GetAllAsync(new PageViewModel()
            {
                SortProperty = sortExpression,
                SortOrder = sortOrder,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                IsLocked = false
            });

            var pager = new Pager(users.TotalRecords, pageIndex, pageSize);
            pager.SortExpression = sortExpression;
            ViewBag.Pager = pager;

            TempData["CurrentPage"] = pageIndex;
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Sort(string keyword, SortOrder sortOrder = 0, int pageIndex = 1, int pageSize = 5, string sortExpression = "id")
        {
            return Json(new
            {
                isValid = true,
                html = Helper.RenderRazorViewToString(
                    this,
                    "_IndexPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel()
                    {
                        SortProperty = sortExpression,
                        SortOrder = sortOrder,
                        Keyword = keyword,
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        IsLocked = false
                    }))
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UsersDeletedList(string keyword, int pageIndex = 1, int pageSize = 5, string sortExpression = "id")
        {
            var users = await _userApiClient.GetAllAsync(new PageViewModel()
            {
                SortProperty = sortExpression,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                IsLocked = true
            });
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            return View(await _userApiClient.GetUserByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            return View(await _userApiClient.GetUserByNameAsync(username));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var response = await _userApiClient.CreateAsync(request);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Create user success";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Create user fail";
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userApiClient.GetUserByIdAsync(id);
            return View(new UpdateUserRequest()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var response = await _userApiClient.UpdateAsync(id, request);
            if (response.IsSuccess)
            {
                var user = await _userApiClient.GetUserByIdAsync(id);
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_AdminProfilePartialView",
                    user)
                });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Update", request) });
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePassword(int id)
        {
            if (await _userApiClient.GetUserByIdAsync(id) is not null)
            {
                return View();
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(int id, UpdatePasswordRequest request)
        {
            var response = await _userApiClient.UpdatePasswordAsync(id, request);
            if (response.IsSuccess)
            {
                var user = await _userApiClient.GetUserByIdAsync(id);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_AdminProfilePartialView", user) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UpdatePassword", request) });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userApiClient.DeleteAsync(id);
            if (response.IsSuccess)
            {
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_UsersDeletedListPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel
                    {
                        PageIndex = 1,
                        PageSize = 5,
                        IsLocked = true
                    }))
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "_UsersDeletedListPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel
                    {
                        PageIndex = 1,
                        PageSize = 5,
                        IsLocked = true
                    }))
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(int id)
        {
            var response = await _userApiClient.LockAsync(id);
            if (response.IsSuccess)
            {
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_IndexPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel
                    {
                        PageIndex = 1,
                        PageSize = 5,
                        IsLocked = false
                    }))
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "_IndexPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel
                    {
                        PageIndex = 1,
                        PageSize = 5,
                        IsLocked = false
                    }))
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreLockedStatus(int id)
        {
            var response = await _userApiClient.RestoreLockedStatusAsync(id);
            if (response.IsSuccess)
            {
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_UsersDeletedListPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel
                    {
                        PageIndex = 1,
                        PageSize = 5,
                        IsLocked = true
                    }))
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "_UsersDeletedListPartialView",
                    await _userApiClient.GetAllAsync(new PageViewModel
                    {
                        PageIndex = 1,
                        PageSize = 5,
                        IsLocked = true
                    }))
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke()
        {
            await _userApiClient.RevokeAsync();
            return Json(new
            {
                isValid = true,
                html = Helper.RenderRazorViewToString(this, "_IndexPartialView",
                await _userApiClient.GetAllAsync(new PageViewModel
                {
                    SortProperty = "id",
                    SortOrder = 0,
                    PageIndex = 1,
                    PageSize = 5,
                    IsLocked = false
                }))
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RevoleByUsername(string username)
        {
            var response = await _userApiClient.RevokeByUsernameAsync(username);
            var users = await _userApiClient.GetAllAsync(new PageViewModel
            {
                SortProperty = "id",
                SortOrder = 0,
                PageIndex = 1,
                PageSize = 5,
                IsLocked = false
            });
            if (response.IsSuccess)
            {
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_IndexPartialView",
                    users
                )
                });
            }
            return Json(new
            {
                isValid = false,
                html = Helper.RenderRazorViewToString(this, "_IndexPartialView",
                users
                )
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetRole(int id)
        {
            return View(await _userApiClient.GetRolesByUserIdAsync(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(int id, string role)
        {
            var response = await _userApiClient.AssignRoleAsync(id, role);
            if (response.IsSuccess)
            {
                TempData["Message"] = $"Assign role user with {id} success";
                return View("Index");
            }
            TempData["Error"] = $"Assign role user with {id} fail";
            return View("Update", new { role });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(int id, string role)
        {
            var response = await _userApiClient.RemoveRoleAsync(id, role);
            if (response.IsSuccess)
            {
                TempData["Message"] = $"Remove role user with {id} success";
                return View("Index");
            }
            TempData["Error"] = $"Remove role user with {id} fail";
            return View("Update", new { role });
        }
    }
}
