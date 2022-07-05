﻿using AutoMapper;
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

        public async Task<bool> AssignRoleAsync(int id, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return false;
            }
            await _userManager.AddToRolesAsync(user, roles);
            return true;
        }

        public async Task<CreateResponse> CreateAsync(CreateUserDto createUserDto)
        {
            string errors = "";
            if (createUserDto is null)
            {
                throw new BadHttpRequestException(Exceptions.BadRequest);
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
                return new CreateResponse(errors);
            }
            var user = _mapper.Map<AppUser>(createUserDto);
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
                            await _roleManager.CreateAsync(new AppRole() { Name = Roles.Member });
                        }
                        await _userManager.AddToRoleAsync(user, Roles.Member);
                        var roles = await _userManager.GetRolesAsync(user);
                        userDto.Roles = roles;
                        _mapper.Map(user, userDto);
                        await trans.CommitAsync();
                        return new CreateResponse(userDto, Messages.CreateSuccess);
                    }
                    return new CreateResponse(Exceptions.CreateFail);
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
                return new CreateResponse(Exceptions.CreateFail);
            }
            return await init.ExecuteAsync(Values);
        }

        public async Task<bool> DeLeteAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new BadHttpRequestException(Exceptions.UserNotFound);
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<IList<AppUserDto>> GetAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<AppUserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<AppUserDto>(user);
                userDto.Roles = roles;
                result.Add(userDto);
            }
            return result;
        }

        public async Task<AppUserDto> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new BadHttpRequestException(Exceptions.UserNotFound);
            }
            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<AppUserDto> GetUserByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                throw new BadHttpRequestException(Exceptions.UserNotFound);
            }
            return _mapper.Map<AppUserDto>(user);
        }

        public async Task<Response> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return new Response()
                {
                    IsSuccess = false,
                    Message = Exceptions.EmailNotFound
                };
            }
            var res = await _userManager.GetLockoutEndDateAsync(user);
            DateTimeOffset? dateTimeOffset = res.HasValue ? res : null;
            if (dateTimeOffset != null)
            {
                _ = int.TryParse(_configuration["ValidateJwt:DefaultLockoutTimeSpan"], out int lockoutTime);
                if (dateTimeOffset?.AddMinutes(lockoutTime) > DateTime.Now)
                {
                    return new Response()
                    {
                        IsSuccess = false,
                        Message = Exceptions.EmailBlocked
                    };
                }
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);
            if (!result.Succeeded)
            {
                return new Response()
                {
                    IsSuccess = false,
                    Message = Exceptions.LoginFail
                };
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
            _ = int.TryParse(_configuration["Jwt:RefreshTokenValidityInDays"], out int expRefreshToken);

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return new Response()
            {
                IsSuccess = true,
                Message = Messages.LoginSuccess,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResult> RefreshTokenAsync(TokenDto tokenDto)
        {
            if (tokenDto is null)
            {
                throw new BadHttpRequestException(Exceptions.InvalidRequest);
            }
            string? accessToken = tokenDto.AccessToken;
            string? refreshToken = tokenDto.RefreshToken;
            var principal = TokenValidation(accessToken);
            if (principal is null)
            {
                throw new BadHttpRequestException(Exceptions.ValidateTokenFail);
            }
            var user = await _userManager.FindByNameAsync(principal.Identity?.Name);
            if (user is null || user.RefreshToken != refreshToken)
            {
                throw new BadHttpRequestException(Exceptions.InvalidToken);
            }
            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = user.RefreshToken;

            return new TokenResult()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> RemoveRoleAsync(int id, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return false;
            }
            await _userManager.RemoveFromRolesAsync(user, roles);
            return true;
        }

        public async Task<bool> RevokeAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                return false;
            }
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task RevokeAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new BadHttpRequestException(Exceptions.NullId);
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                throw new Exception(Exceptions.UserNotFound);
            }
            var data = _mapper.Map(updateUserDto, user);
            var init = _appDbContext.Database.CreateExecutionStrategy();
            async Task<bool> Values()
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    await _userManager.ChangePasswordAsync(user, updateUserDto.CurrentPassword, updateUserDto.NewPassword);
                    var res = await _userManager.UpdateAsync(data);
                    if (res.Succeeded)
                    {
                        await trans.CommitAsync();
                        return true;
                    }
                    return false;
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
            _ = int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int expTime);

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
                throw new SecurityTokenException(Exceptions.InvalidToken);
            }
            return principal;
        }
    }
}