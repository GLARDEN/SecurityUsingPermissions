using AutoMapper;
using Ardalis.GuardClauses;
using Security.Core.Permissions.Enums;
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Permissions.Extensions;
using Security.SharedKernel.Interfaces;
using Security.Core.Models.Administration.RoleManagement.Specifications;
using Ardalis.Result;
using Security.Core.Models.Administration.RoleManagement.Exceptions;

namespace Security.Core.Models.Administration.RoleManagement.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Role> _repository;

    private readonly Type _permissionEnumType;

    public RoleService(IMapper mapper, IRepository<Role> repository)
    {
        _mapper = mapper;
        _repository = repository;
        _permissionEnumType = typeof(Permission);
    }

    public async Task<Result<CreateRoleResponse>> CreateAsync(CreateRoleRequest createRoleRequest)
    {
        try
        {
            Role newRole = Role.Create(createRoleRequest.Name, createRoleRequest.Description , true, createRoleRequest.Permissions.PackPermissionsNames());
            
            var result = await _repository.AddAsync(newRole);

            CreateRoleResponse response = new()
            {
                RoleId = result.Id
            };

            return Result<CreateRoleResponse>.Success(response);
        }
        catch (AggregateException ex) when ((ex.InnerException) is DuplicateRoleNameException duplicateRoleNameException)
        {
            ValidationError validationError = new ValidationError()
            {
                Severity = ValidationSeverity.Warning,
                Identifier = createRoleRequest.Name,
                ErrorMessage = $"{duplicateRoleNameException.RoleName} already exists"
            };
            return Result<CreateRoleResponse>.Invalid(new List<ValidationError>() { validationError });
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException || ex is ArgumentNullException)
            {
                return Result<CreateRoleResponse>.Error(ex.Message);
            }
            else
            {
                return Result<CreateRoleResponse>.Error(ex.Message);
            }
        }
    }

    public async Task<Result<DeleteRoleResponse>> Delete(DeleteRoleRequest deleteRoleRequest)
    {
        Guard.Against.Null(deleteRoleRequest, nameof(deleteRoleRequest), "Delete request object is required.");
        Guard.Against.Default(deleteRoleRequest.RoleId, nameof(deleteRoleRequest.RoleId), "Request must contain a valid role id");


        var role = await _repository.GetByIdAsync(deleteRoleRequest.RoleId);
       
        if (role != null)
        {
           await _repository.DeleteAsync(role, CancellationToken.None);
        }

        if (role == null)
        {
            return Result<DeleteRoleResponse>.Success(new DeleteRoleResponse()
            {
                Success = true
            });
        }
        else
        {
            return Result<DeleteRoleResponse>.Error($"Failed to delete role {role?.Name}.");
        }
    }

    public async Task<Result<ListRolesResponse>> ListAsync()
    {
        try
        {
            var getEnabledRolesSpec = new GetEnabledRolesSpec();
            var roleList = await _repository.ListAsync(getEnabledRolesSpec);

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
            Role? role = await _repository.GetBySpecAsync(new GetRoleByIdSpec(request.Id));

            if (role == null)
            {
                return Result<UpdateRoleResponse>.NotFound();
            }
            else
            {
                role.RenameRole(request.Name);
                role.ChangeRoleDescription(request.Description);
                role.UpdatePermissions(request.PermissionNames.PackPermissionsNames());

                await _repository.UpdateAsync(role);

                return Result<UpdateRoleResponse>.Success(new UpdateRoleResponse()); 
            }
        }
        catch (Exception ex)
        {
            var errorList = new List<ValidationError>() { new ValidationError { Identifier = nameof(Role), ErrorMessage = ex.InnerException.ToString() } };
            return Result<UpdateRoleResponse>.Invalid(errorList);
        }
    }

    public async Task<bool> RoleExists(string roleName)
    {
        bool roleExists = await _repository.AnyAsync(new GetRoleByNameSpec(roleName));
        return roleExists;
    }
}


