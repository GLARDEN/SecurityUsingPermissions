using AutoMapper;
using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions.Extensions;
using Ardalis.Result;
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
            Guard.Against.Null(createRoleRequest,nameof(createRoleRequest));

            if (await RoleExists(createRoleRequest.Name))
            {
                return Result<CreateRoleResponse>.Error("Role already exists.");
            }

            Role newRole = new(createRoleRequest.Name, createRoleRequest.Description, true, createRoleRequest.Permissions.PackPermissionsNames());

            await _appDbContext.Roles.AddAsync(newRole);

            var result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                return Result<CreateRoleResponse>.Success(new CreateRoleResponse()
                {   
                    Role = _mapper.Map<RoleDto>(newRole)
                });
            }
            else
            {
                return Result<CreateRoleResponse>.Error($"Failed to save role {newRole.Name}.");
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
        Guard.Against.Default(deleteRoleRequest.RoleId, nameof(deleteRoleRequest.RoleId), "Request must contain a valid role id");

        var role = _appDbContext.Roles.Find(deleteRoleRequest.RoleId);
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
            return Result<DeleteRoleResponse>.Error($"Failed to delete role {role.Name}.");
        }
    }

    public async Task<Result<ListRolesResponse>> ListAsync()
    {
        try
        {
            var roleList = await _appDbContext.Roles.Where(r => r.Enabled).ToListAsync();

            return Result<ListRolesResponse>.Success(new ListRolesResponse()
            {   
                Roles = _mapper.Map<List<RoleDto>>(roleList) ?? new List<RoleDto>()
            });

        }
        catch (Exception ex)
        {
            return Result<ListRolesResponse>.Error(ex.Message);
        }
    }

    public async Task<Result<UpdateRoleResponse>> UpdateAsync(UpdateRoleRequest request)
    {
        try
        {
            Role? role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.Id);

            if (role == null)
            {
                return Result<UpdateRoleResponse>.NotFound();
            }
            else
            {
                role.RenameRole(request.Name);
                role.ChangeRoleDescription(request.Description);
                role.UpdatePermissions(request.PermissionNames);

                await _appDbContext.SaveChangesAsync();

                return Result<UpdateRoleResponse>.Success(new UpdateRoleResponse()
                {
                    Role = _mapper.Map<RoleDto>(role)
                });
            }
        }
        catch (Exception ex)
        {
            return Result<UpdateRoleResponse>.Error(ex.Message);
        }
    }

    public async Task<bool> RoleExists(string roleName)
    {
        bool roleExists = await _appDbContext.Roles.AnyAsync(r => r.Name.ToLower() == roleName.ToLower());
        return roleExists;
    }
}


