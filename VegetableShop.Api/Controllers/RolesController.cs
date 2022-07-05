using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Role;
using VegetableShop.Api.Services.Role;

namespace VegetableShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IConfiguration _configuration;
        public RolesController(IRoleService roleService, IConfiguration configuration)
        {
            _roleService = roleService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_roleService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoleDto createRoleDto)
        {
            var result = await _roleService.CreateAsync(createRoleDto);
            if (result.IsSuccess)
            {
                return Created(new Uri($"{_configuration["BaseAddress"]}/api/roles/{result.appRoleDto.Id}"), result.appRoleDto);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            if (await _roleService.GetRoleByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _roleService.UpdateAsync(id, updateRoleDto))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _roleService.GetRoleByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _roleService.DeleteAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
