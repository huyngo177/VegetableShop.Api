using Microsoft.AspNetCore.Authentication.Cookies;
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
                          policy.WithOrigins("https://localhost:7230", "https://localhost:7157").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                      });
});
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/User/Forbidden/";
    });

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddHttpContextAccessor();
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

//app.MapControllerRoute(
//    name: "MyArea",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=UserHome}/{action=Index}/{id?}");

app.UseCors("_allowSpecificOrigins");
app.Run();
