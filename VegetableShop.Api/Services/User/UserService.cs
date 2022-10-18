using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VegetableShop.Api.Common;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Api.Dto.User;
namespace VegetableShop.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager,
            IConfiguration configuration, IMapper mapper, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<bool> AssignRoleAsync(int id, string role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<CreateResponse> CreateAsync(CreateUserDto createUserDto)
        {
            string errors = "";
            if (createUserDto is null)
            {
                throw new AppException(Exceptions.BadRequest);
            }
            if (await _userManager.FindByEmailAsync(createUserDto.Email) is not null)
            {
                errors = string.Concat(errors, Exceptions.EmailExist);
            }
            if (await _userManager.FindByNameAsync(createUserDto.UserName) is not null)
            {
                errors = string.Concat(errors, Exceptions.UsernameExist);
            }
            if (errors.Length > 0)
            {
                throw new AppException(errors);
            }
            var user = _mapper.Map<AppUser>(createUserDto);
            user.IsLocked = false;
            var userDto = new AppUserDto();
            var init = _appDbContext.Database.CreateExecutionStrategy();
            async Task<CreateResponse> Values()
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    var result = await _userManager.CreateAsync(user, createUserDto.Password);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(Roles.Member))
                        {
                            await _roleManager.CreateAsync(new AppRole()
                            {
                                Name = Roles.Member,
                                Description = Roles.MemberDesc
                            });
                        }
                        await _userManager.AddToRoleAsync(user, Roles.Member);
                        var roles = await _userManager.GetRolesAsync(user);
                        userDto.Roles = roles;
                        _mapper.Map(user, userDto);
                        await trans.CommitAsync();
                        return new CreateResponse(userDto, Messages.CreateSuccess);
                    }
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                }
                throw new AppException(Exceptions.CreateFail);
            }
            return await init.ExecuteAsync(Values);
        }

        public async Task<bool> DeLeteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<PaginationResult<AppUserDto>> GetAsync(PageDto pageDto)
        {
            List<AppUser> query = await _appDbContext.Users.Where(x => x.UserName != SystemConstants.AdminName).ToListAsync();

            if (pageDto.IsLocked)
            {
                query = query.Where(x => x.IsLocked == pageDto.IsLocked).ToList();
            }
            if (!string.IsNullOrEmpty(pageDto.Keyword))
            {
                query = query.Where(x => x.UserName.ToLower().Contains(pageDto.Keyword.ToLower())
                 || x.PhoneNumber.Contains(pageDto.Keyword)).ToList();
            }
            query = DoSort(query, pageDto.SortProperty, pageDto.SortOrder);
            var result = new List<AppUserDto>();
            foreach (var user in query)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<AppUserDto>(user);
                userDto.Roles = roles;
                result.Add(userDto);
            }

            var pageResult = new PaginationResult<AppUserDto>(result, pageDto.PageIndex, pageDto.PageSize);           ;
            
            return pageResult;
        }

        public async Task<AppUserDto> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new KeyNotFoundException(Exceptions.UserNotFound);
            }
            var result = _mapper.Map<AppUserDto>(user);
            result.Roles = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<AppUserDto> GetUserByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                throw new KeyNotFoundException(Exceptions.UserNotFound);
            }
            var result = _mapper.Map<AppUserDto>(user);
            result.Roles = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<Response> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                throw new KeyNotFoundException(Exceptions.EmailNotFound);
            }
            if (user.IsLocked)
            {
                throw new AppException(Exceptions.UserNotExist);
            }
            var res = await _userManager.GetLockoutEndDateAsync(user);
            DateTimeOffset? dateTimeOffset = res.HasValue ? res : null;
            if (dateTimeOffset != null)
            {
                _ = int.TryParse(_configuration["IdentityConfig:LockoutDefaultLockoutTimeSpan"], out int lockoutTime);
                if (dateTimeOffset?.AddMinutes(lockoutTime) > DateTime.Now)
                {
                    throw new AppException(Exceptions.EmailBlocked);
                }
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            if (!result.Succeeded)
            {
                throw new AppException(Exceptions.LoginFail);
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = CreateToken(claims);
            var refreshToken = CreateRefreshToken();

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new Response()
            {
                IsSuccess = true,
                Message = Messages.LoginSuccess,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                UserId = user.Id
            };
        }

        public async Task<TokenResult> RefreshTokenAsync(TokenDto tokenDto)
        {
            if (tokenDto is null)
            {
                throw new AppException(Exceptions.InvalidRequest);
            }
            string? accessToken = tokenDto.AccessToken;
            string? refreshToken = tokenDto.RefreshToken;
            var principal = TokenValidation(accessToken);
            if (principal is null)
            {
                throw new AppException(Exceptions.ValidateTokenFail);
            }
            var user = await _userManager.FindByNameAsync(principal.Identity?.Name);
            if (user is null || user.RefreshToken != refreshToken)
            {
                throw new AppException(Exceptions.InvalidToken);
            }
            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = user.RefreshToken;

            return new TokenResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> RemoveRoleAsync(int id, string role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.RemoveFromRoleAsync(user, role);
            return true;
        }

        public async Task<bool> RevokeAsync(string username)
        {
            if (username.ToUpper() == SystemConstants.AdminName.ToUpper())
            {
                return false;
            }
            var user = await _userManager.FindByNameAsync(username);
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task RevokeAllAsync()
        {
            var users = await _userManager.Users.Where(x => x.UserName != SystemConstants.AdminName).ToListAsync();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            if (updateUserDto is null)
            {
                throw new AppException(Exceptions.BadRequest);
            }
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new AppException(Exceptions.NullId);
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user.IsLocked)
            {
                return false;
            }
            _mapper.Map(updateUserDto, user);
            var init = _appDbContext.Database.CreateExecutionStrategy();
            async Task<bool> Values()
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    var res = await _userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        await trans.CommitAsync();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
                return false;
            }
            return await init.ExecuteAsync(Values);
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            _ = int.TryParse(_configuration["ValidateJwt:TokenValidityInMinutes"], out int expTime);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(expTime),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
        private string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? TokenValidation(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AppException(Exceptions.InvalidToken);
            }
            return principal;
        }

        public async Task<bool> ChangeLockedStatusAsync(int id, bool isLocked)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                return false;
            }
            user.IsLocked = isLocked;
            user.RefreshToken = null;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto)
        {
            if (updatePasswordDto is null)
            {
                throw new AppException(Exceptions.BadRequest);
            }
            if (updatePasswordDto.CurrentPassword.Equals(updatePasswordDto.NewPassword))
            {
                throw new AppException(Exceptions.PasswordInvalid);
            }
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new AppException(Exceptions.NullId);
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user.IsLocked)
            {
                return false;
            }
            var init = _appDbContext.Database.CreateExecutionStrategy();
            async Task<bool> Values()
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    var result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
                    if (result.Succeeded)
                    {
                        var res = await _userManager.UpdateAsync(user);
                        if (res.Succeeded)
                        {
                            await trans.CommitAsync();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
                throw new AppException(Exceptions.CurrentPasswordInvalid);
            }
            return await init.ExecuteAsync(Values);
        }

        public async Task<IEnumerable<string>> GetRolesByUserIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return await _userManager.GetRolesAsync(user);
        }

        private List<AppUser> DoSort(List<AppUser> query, string sortProperty, SortOrder sortOrder)
        {
            if (sortProperty.ToLower() == "username")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(x => x.UserName).ToList();
                }
                else
                {
                    query = query.OrderByDescending(x => x.UserName).ToList();
                }
            }
            else if (sortProperty.ToLower() == "id")
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(x => x.Id).ToList();
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id).ToList();
                }
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(x => x.Email).ToList();
                }
                else
                {
                    query = query.OrderByDescending(x => x.Email).ToList();
                }
            }
            return query;
        }
    }
}
