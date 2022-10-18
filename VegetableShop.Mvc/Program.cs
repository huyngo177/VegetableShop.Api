using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VegetableShop.Api.Mapper.User;
using VegetableShop.Mvc.ApiClient.Carts;
using VegetableShop.Mvc.ApiClient.Categories;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.ApiClient.Role;
using VegetableShop.Mvc.ApiClient.User;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_allowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7230", "https://localhost:7157")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowAnyOrigin();
                      });
});
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Home/Forbidden/";
    });

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserApiClient, UserApiClient>();
builder.Services.AddScoped<IProductApiClient, ProductApiClient>();
builder.Services.AddScoped<IRoleApiClient, RoleApiClient>();
builder.Services.AddScoped<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddScoped<ICartApiClient, CartApiClient>();
builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseCookiePolicy();

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "admin",
        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    routes.MapRoute(
        name: "default",
        template: "{controller=UserHome}/{action=Index}/{id?}");
});

app.UseCors("_allowSpecificOrigins");
app.Run();
