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
using Ardalis.Result;
using Security.Shared.Models.UserManagement;
using Ardalis.GuardClauses;

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
        _permissionEnumType = typeof(Permission);
    }

    public async Task<Result<CreateRoleResponse>> Create(CreateRoleRequest createRoleRequest)
    {
        try
        {
            if (await RoleExists(createRoleRequest.Name))
            {
                return Result<CreateRoleResponse>.Error("Role already exists.");
            }

            Role newRole = new(createRoleRequest.Name, createRoleRequest.Description, true, createRoleRequest.Permissions);

            await _appDbContext.Roles.AddAsync(newRole);

            var result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                return Result<CreateRoleResponse>.Success(new CreateRoleResponse()
                {
                    Success = true,
                    Role = _mapper.Map<RoleDto>(newRole)
                });
            }
            else
            {
                return Result<CreateRoleResponse>.Error($"Failed to save role {newRole.RoleName}.");
            }
        }
        catch (Exception ex)
        {
            return Result<CreateRoleResponse>.Error(ex.Message);
        }
    }

    public async Task<Result<DeleteRoleResponse>> Delete(DeleteRoleRequest deleteRoleRequest)
    {
        Guard.Against.Null(deleteRoleRequest, nameof(deleteRoleRequest), "Delete request object is required.");
        Guard.Against.Null(deleteRoleRequest.Role, nameof(deleteRoleRequest.Role), "Request must contain a role object");

        var role = _appDbContext.Roles.Find(deleteRoleRequest.Role.RoleName);
        if (role != null)
        {
            _appDbContext.Roles.Remove(role);
        }

        var result = await _appDbContext.SaveChangesAsync();

        if (result > 0)
        {
            return Result<DeleteRoleResponse>.Success(new DeleteRoleResponse()
            {
                Success = true
            });
        }
        else
        {
            return Result<DeleteRoleResponse>.Error($"Failed to delete role {deleteRoleRequest.Role.RoleName}.");
        }
    }

    public async Task<Result<ListRolesResponse>> ListAsync()
    {
        try
        {
            var roleList = await _appDbContext.Roles.Where(r => r.Enabled).ToListAsync();

            return Result<ListRolesResponse>.Success(new ListRolesResponse()
            {
                Success = true,
                Roles = _mapper.Map<List<RoleDto>>(roleList) ?? new List<RoleDto>()
            });

        }
        catch (Exception ex)
        {
            return Result<ListRolesResponse>.Error(ex.Message);
        }
    }

    public async Task<Result<EditRoleResponse>> EditAsync(EditRoleRequest request)
    {
        try
        {
            Role role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == request.RoleName);

            if (role == null)
            {
                return Result<EditRoleResponse>.NotFound();
            }
            else
            {                
                role.UpdatePermissions(request.PermissionNames);

                await _appDbContext.SaveChangesAsync();

                return Result<EditRoleResponse>.Success(new EditRoleResponse()
                {
                    Success = true,                  
                });
            }
        }
        catch (Exception ex)
        {
            return Result<EditRoleResponse>.Error(ex.Message);
        }
    }

    public async Task<bool> RoleExists(string roleName)
    {
        bool roleExists = await _appDbContext.Roles.AnyAsync(r => r.RoleName.ToLower() == roleName.ToLower());
        return roleExists;
    }
}


