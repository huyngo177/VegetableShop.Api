using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Mapper.Categories;
using VegetableShop.Api.Mapper.Products;
using VegetableShop.Api.Mapper.Role;
using VegetableShop.Api.Mapper.User;
using VegetableShop.Api.Models.Validations.User;
using VegetableShop.Api.Services.Categories;
using VegetableShop.Api.Services.Products;
using VegetableShop.Api.Services.Role;
using VegetableShop.Api.Services.Storage;
using VegetableShop.Api.Services.User;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_allowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7230", "https://localhost:7157").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                      });
});
builder.Services.AddEndpointsApiExplorer();
//DB Context
builder.Services.AddDbContext<AppDbContext>(
    options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies()
    );

//Identity
builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

//DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<SignInManager<AppUser>>();
builder.Services.AddScoped<RoleManager<AppRole>>();

//Auto Mapper
builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddAutoMapper(typeof(ProductMapping));
builder.Services.AddAutoMapper(typeof(CategoryMapping));
builder.Services.AddAutoMapper(typeof(RoleMapping));

//Fluent API
builder.Services.AddControllers()
                .AddFluentValidation(opts =>
                    opts.RegisterValidatorsFromAssembly(typeof(CreateAppUserValidator).Assembly)
                );

//Configure Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 2;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

//SQL Local
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.UseCors("_allowSpecificOrigins");

app.Run();
