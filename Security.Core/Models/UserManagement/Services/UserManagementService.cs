     using Ardalis.Result;

using AutoMapper;

using Security.Core.Models.Administration.RoleManagement.Exceptions;
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.UserManagement.Specifications;
using Security.SharedKernel.Interfaces;
using Security.Core.Services;
using Security.Core.Models.UserManagement.Exceptions;
using Security.Core.Permissions.Extensions;
using Ardalis.GuardClauses;

namespace Security.Core.Models.UserManagement.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IMapper _mapper;
    private readonly IRepository<User> _repository;
    private readonly IHashService _hashService;

    public UserManagementService(IMapper mapper, IRepository<User> repository, IHashService hashService)
    {
        _mapper = mapper;
        _repository = repository;
        _hashService = hashService;
    }

    public async Task<Result<CreateUserResponse>> CreateAsync(CreateUserRequest createUserRequest)
    {
        try
        {
            Guard.Against.NullOrEmpty(createUserRequest.TemporaryPassword, nameof(createUserRequest.TemporaryPassword), "Newly created user must have temporary password set.");

            _hashService.CreateHash(createUserRequest.TemporaryPassword, out byte[] passwordHash, out byte[] passwordSalt);

            User newUser = User.Create(createUserRequest.Email, passwordHash, passwordSalt, createUserRequest.Enabled);

            createUserRequest.AssignedRoles.ForEach(ar => 
            {
                newUser.AssignRole(ar.RoleName, ar.AssignedPermissions.PackPermissionsNames());
            });
            

            var result = await _repository.AddAsync(newUser);

            CreateUserResponse response = new()
            {
                UserId = result.Id
            };

            return Result<CreateUserResponse>.Success(response);
        }
        catch (AggregateException ex) when ((ex.InnerException) is DuplicateEmailException duplicateEmailException)
        {

            ValidationError validationError = new ValidationError()
            {
                Severity = ValidationSeverity.Warning,
                Identifier = createUserRequest.Email,
                ErrorMessage = $"{duplicateEmailException.EmailAddress} already exists"
            };

            return Result<CreateUserResponse>.Invalid(new List<ValidationError>() { validationError });
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException || ex is ArgumentNullException)
            {
                return Result<CreateUserResponse>.Error(ex.Message);
            }
            else
            {
                return Result<CreateUserResponse>.Error(ex.Message);
            }
        }
    }

    public async Task<Result<ListUsersResponse>> ListAsync()
    {
        try
        {
            var userList = await _repository.ListAsync(new GetUsersIncludeAssignedRolesAndTokensSpec());

            var users = _mapper.Map<List<UserDto>>(userList);

            return Result<ListUsersResponse>.Success(new ListUsersResponse()
            {
                Success = true,
                RegisteredUsers = users ?? new List<UserDto>()
            });
        }
        catch (Exception ex)
        {
            return Result<ListUsersResponse>.Error(ex.Message);
        }
    }

    public async Task<Result<UpdateUserResponse>> UpdateAsync(UpdateUserRequest request)
    {
        UpdateUserResponse updateUserResponse = new();
        try
        {            
            User userToUpdate = await _repository.GetBySpecAsync(new GetUserAndAssignedRolesByIdSpec(request.Id));
            if (userToUpdate != null)
            {
                userToUpdate.UpdateEmail(request.Email);

                request.Roles.ToList().ForEach(async r =>
                {
                    if (r.IsDeleted)
                    {
                       var userRoleToDelete = userToUpdate.UserRoles.FirstOrDefault(ur => ur.RoleName == r.RoleName);
                        if(userRoleToDelete != null)
                        {
                            userToUpdate.RevokeRole(userRoleToDelete);
                        }
                    }
                    else
                    {
                        if (!userToUpdate.UserRoles.Any(ur => ur.RoleName == r.RoleName))
                        {
                            userToUpdate?.AssignRole(r.RoleName, r.AssignedPermissions.PackPermissionsNames());
                        }
                        else
                        {
                            userToUpdate?.UpdateRole(r.RoleName, r.AssignedPermissions.PackPermissionsNames());
                        }
                    }

                });

                await _repository.UpdateAsync(userToUpdate);  

                UserDto user = _mapper.Map<UserDto>(userToUpdate);
                updateUserResponse.User = user;
            };
            return Result<UpdateUserResponse>.Success(updateUserResponse);
        }
        catch (Exception ex)
        {
            return Result<UpdateUserResponse>.Error(ex.Message);
        }


    }  
}
