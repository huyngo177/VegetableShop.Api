using Microsoft.AspNetCore.Authentication.Cookies;
using VegetableShop.Api.Mapper.User;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.ApiClient.User;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden/";
    });

builder.Services.AddScoped<IUserApiClient, UserApiClient>();
builder.Services.AddScoped<IProductApiClient, ProductApiClient>();
builder.Services.AddAutoMapper(typeof(UserMapping));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMvc();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
