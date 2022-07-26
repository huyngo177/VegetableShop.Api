using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using VegetableShop.Api.Common;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Mapper.Categories;
using VegetableShop.Api.Mapper.Products;
using VegetableShop.Api.Mapper.Role;
using VegetableShop.Api.Mapper.User;
using VegetableShop.Api.Models.Validations.User;
using VegetableShop.Api.Services.Categories;
using VegetableShop.Api.Services.Orders;
using VegetableShop.Api.Services.Products;
using VegetableShop.Api.Services.Role;
using VegetableShop.Api.Services.Storage;
using VegetableShop.Api.Services.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

_ = int.TryParse(builder.Configuration.GetSection("IdentityConfig:PasswordRequiredLength").Value, out int minLength);
_ = int.TryParse(builder.Configuration.GetSection("IdentityConfig:PasswordRequiredUniqueChars").Value, out int minChars);
_ = int.TryParse(builder.Configuration.GetSection("IdentityConfig:LockoutDefaultLockoutTimeSpan").Value, out int lockoutTime);
_ = int.TryParse(builder.Configuration.GetSection("IdentityConfig:LockoutMaxFailedAccessAttempts").Value, out int accessAttempts);

//DB Context
builder.Services.AddDbContext<AppDbContext>(
    options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.ConnectionString))
                .UseLazyLoadingProxies()
    );

//Identity
builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>();
//DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IOrderService, OrderService>();
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
                )
                .AddJsonOptions(
                    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles

                );

//Configure Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = minLength;
    options.Password.RequiredUniqueChars = minChars;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(lockoutTime);
    options.Lockout.MaxFailedAccessAttempts = accessAttempts;

    options.User.AllowedUserNameCharacters = builder.Configuration.GetSection("IdentityConfig:UserAllowedUserNameCharacters").Value;
    options.User.RequireUniqueEmail = true;
});

//Ad Authenticate
builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:SecretKey").Value)),
            ClockSkew = TimeSpan.Zero
        };
    });

//Add Authorize
//builder.Services.AddAuthorization(opt =>
//{
//    opt.AddPolicy("AdminPolicy",
//      policy =>
//      {
//          policy.RequireClaim(ClaimTypes.Role, Roles.Admin);
//      });
//    opt.AddPolicy("MemberPolicy",
//        policy => policy.RequireClaim(ClaimTypes.Role, Roles.Member));
//});

//Add Swagger
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = "ASP.NET Core 6.0 Web API"
    });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});

var app = builder.Build();

//Declare DB if not exist
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

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();
