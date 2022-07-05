using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;
using VegetableShop.Api.Common;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Role;

namespace VegetableShop.Api.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<CreateResponse> CreateAsync(CreateRoleDto createRoleDto)
        {
            if (createRoleDto is null)
            {
                throw new BadHttpRequestException(Exceptions.BadRequest);
            }
            if (await _roleManager.FindByNameAsync(createRoleDto.Name) is not null)
            {
                throw new BadHttpRequestException(Exceptions.RoleNameExist);
            }
            var roleDto = _mapper.Map<AppRole>(createRoleDto);
            var result = await _roleManager.CreateAsync(roleDto);
            if (result.Succeeded)
            {
                return new CreateResponse(_mapper.Map<AppRoleDto>(roleDto), Messages.CreateSuccess);
            }
            return new CreateResponse(Exceptions.CreateFail);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
                return false;
            }
            await _roleManager.DeleteAsync(role);
            return true;
        }

        public IEnumerable<AppRoleDto> GetAll()
        {
            var roles = _roleManager.Roles.ToList();
            IEnumerable<AppRoleDto> result = _mapper.Map<List<AppRole>, IEnumerable<AppRoleDto>>(roles);
            return result;
        }

        public async Task<AppRoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
                return null;
            }
            return _mapper.Map<AppRoleDto>(role);
        }

        public async Task<bool> UpdateAsync(int id, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null)
            {
                return false;
            }
            var roleDto = _mapper.Map(updateRoleDto, role);
            var result = await _roleManager.UpdateAsync(roleDto);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
