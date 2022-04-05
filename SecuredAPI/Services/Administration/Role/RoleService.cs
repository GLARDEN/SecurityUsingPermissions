using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions.Extensions;

namespace SecuredAPI.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly Type _permissionEnumType;
    
    public RoleService(IMapper mapper, AppDbContext appDbContext)
    {
        _mapper = mapper;
        _appDbContext = appDbContext;
        _permissionEnumType =  typeof(Permission); 
    }


    public async Task<CreateRoleResponse> Create(CreateRoleRequest createRoleRequest)
    {

        if (await RoleExists(createRoleRequest.Name))
        {
            return new CreateRoleResponse()
            {
                Success = false,
                ErrorMessage = "Role Already Exists."
            };
        }

        string permissions = _permissionEnumType.PackPermissionsNames(createRoleRequest.Permissions);

        Role newRole = new Role(createRoleRequest.Name, createRoleRequest.Description,true, permissions );

        _appDbContext.Roles.Add(newRole);

        var result = await _appDbContext.SaveChangesAsync();

        CreateRoleResponse _response = new CreateRoleResponse()
        {
            Success = result > 0 ? true : false,
            ErrorMessage = result! > 0 ? "Failed to save role" : "",
            Role = _mapper.Map<RoleDto>(newRole)
        };

        return _response;
    }

    public async Task<DeleteRoleResponse> Delete(DeleteRoleRequest deleteRoleRequest)
    {
        var role = _appDbContext.Roles.Find(deleteRoleRequest.Role.RoleName);
        if (role != null)
        {
            _appDbContext.Roles.Remove(role);
        }

        var result = await _appDbContext.SaveChangesAsync();

        DeleteRoleResponse response = new()
        {
            Success = result == 0 ? false : true
        };

        return response;
    }

    public async Task<ListRolesResponse> GetRolesAsync()
    {
        ListRolesResponse rolesResponse = new();
        rolesResponse.Success = false;

        var roleList = await _appDbContext.Roles.Where(r => r.Enabled).ToListAsync();

        if (roleList != null || roleList?.Count > 0)
        {
            rolesResponse.Roles = _mapper.Map<List<RoleDto>>(roleList);
            rolesResponse.Success = true;
        }

        return rolesResponse;
    }

    public async Task<bool> RoleExists(string roleName)
    {
        bool roleExists = await _appDbContext.Roles.AnyAsync(r => r.RoleName.ToLower() == roleName.ToLower());
        return roleExists;
    }
}
