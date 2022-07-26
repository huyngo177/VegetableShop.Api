using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Api.Dto.User;
using VegetableShop.Api.Services.User;

namespace VegetableShop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsycn([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetAsync([FromQuery] GetUserPageRequest request)
        {
            return Ok(await _userService.GetAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest();
            }
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserDto createUserDto)
        {
            var result = await _userService.CreateAsync(createUserDto);
            if (result.IsSuccess)
            {
                return Created(new Uri($"{_configuration["BaseAddress"]}/api/users/{result.appUserDto.Id}"), result.appUserDto);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (await _userService.GetUserByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _userService.UpdateAsync(id, updateUserDto))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _userService.GetUserByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _userService.ChangeLockedStatusAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{id}/assign-role")]
        [AllowAnonymous]
        public async Task<IActionResult> AssignRoleAsync(int id, [FromBody] string role)
        {
            if (await _userService.GetUserByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _userService.AssignRoleAsync(id, role))
            {
                return Ok(await _userService.GetUserByIdAsync(id));
            }
            return BadRequest();
        }

        [HttpDelete("{id}/remove-role")]
        public async Task<IActionResult> RemoveRoleAsync(int id, [FromBody] string role)
        {
            if (await _userService.GetUserByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _userService.RemoveRoleAsync(id, role))
            {
                return Ok(await _userService.GetUserByIdAsync(id));
            }
            return BadRequest();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenDto tokenDto)
        {
            var result = await _userService.RefreshTokenAsync(tokenDto);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("revoke/{username}")]
        public async Task<IActionResult> RevokeAsync(string username)
        {
            if (await _userService.GetUserByNameAsync(username) is null)
            {
                return NotFound();
            }
            if (await _userService.RevokeAsync(username))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("revokes")]
        public async Task<IActionResult> RevokeAllAsync()
        {
            await _userService.RevokeAllAsync();
            return Ok();
        }

        [HttpPut("{id}/update-password")]
        public async Task<IActionResult> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto)
        {
            if (await _userService.GetUserByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _userService.UpdatePasswordAsync(id, updatePasswordDto))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
