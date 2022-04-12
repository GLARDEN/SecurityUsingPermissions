using Ardalis.Result;

using AutoMapper;

using Security.Core.Models.UserManagement;
using Security.Infrastructure;


namespace SecuredAPI.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;


    public UserManagementService(IMapper mapper, AppDbContext appDbContext)
    {
        _mapper = mapper;
        _appDbContext = appDbContext;
    }

    public async Task<Result<ListUsersResponse>> ListAsync()
    {
        try
        {
            var userList = await _appDbContext.Users.Select(u =>
                                                        new UserSummaryDto
                                                        {
                                                            Id = u.Id,
                                                            Email = u.Email,
                                                            CreatedWhen = u.CreatedWhen,
                                                            RoleNames = u.UserRoles.Where(ur => ur.UserId.Equals(u.Id)).Select(ur => ur.RoleName),
                                                        }).ToListAsync();


            return Result<ListUsersResponse>.Success(new ListUsersResponse()
            {
                Success = true,
                RegisteredUsers = userList ?? new List<UserSummaryDto>()
            });
        }
        catch (Exception ex)
        {
            return Result<ListUsersResponse>.Error(ex.Message);
        }
    }

    public async Task<Result<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest request)
    {
        UpdateUserResponse updateUserResponse = new();
        try
        {
            var userToUpdate = await _appDbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == request.Id);

            if (userToUpdate != null)
            {
                userToUpdate.UpdateEmail(request.Email);

                //request.User.Roles.ForEach(r => { 
                //    var roleToUpdate = userToUpdate?.Roles?.FirstOrDefault(ur => ur.RoleName == r.RoleName);
                //    if(roleToUpdate == null)
                //    {
                //        userToUpdate?.Roles?.Add(new UserRole(request.User.Id, r.RoleName, r.Permissions));
                //    }
                //    else
                //    {
                //        userToUpdate?.UpdateRole(r.RoleName, r.Permissions);
                //    }
                //});
                await _appDbContext.SaveChangesAsync();
             
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

//    public async Task<UpdateUserResponse> EditUserRoles(EditUserRolesRequest request)
//{

//    var userRoles = await _appDbContext.UserRoles.Where(u => u.UserId == request.UserId).ToListAsync();

//    if (!userRoles.Any())
//    {
//        foreach (KeyValuePair<string, IEnumerable<string>> kvp in request.Roles)
//        {
//            UserRole newUserRole = new(request.UserId, kvp.Key, kvp.Value.PackPermissionsNames());

//            _appDbContext.UserRoles.Add(newUserRole);
//        }
//    }
//    else
//    {
//        var userRolesToDelete = userRoles.Where(u => u.UserId == request.UserId && !request.Roles.ContainsKey(u.RoleName)).ToList();
//        var userRolesToEdit = userRoles.Where(u => u.UserId == request.UserId && request.Roles.ContainsKey(u.RoleName)).ToList();

//        var alreadyAssignedRoles = userRoles.Select(ur => ur.RoleName);

//        var userRolesToCreate = request.Roles.Where(r => !alreadyAssignedRoles.Contains(r.Key));

//        if (userRolesToDelete.Any())
//        {
//            _appDbContext.UserRoles.RemoveRange(userRolesToDelete);
//        }

//        if (userRolesToEdit.Any())
//        {
//            request.Roles.ToList().ForEach(r =>
//            {
//                var userRole = userRolesToEdit.FirstOrDefault(ur => ur.RoleName == r.Key);
//                if (userRole != null)
//                {
//                    userRole.UpdatePermissions(r.Value.PackPermissionsNames());
//                }

//            });
//        }

//        if (userRolesToCreate.Any())
//        {
//            foreach (KeyValuePair<string, IEnumerable<string>> kvp in userRolesToCreate)
//            {
//                UserRole newUserRole = new(request.UserId, kvp.Key, kvp.Value.PackPermissionsNames());

//                _appDbContext.UserRoles.Add(newUserRole);
//            }
//        }
//    }

//    var result = await _appDbContext.SaveChangesAsync();

//    UpdateUserResponse response = new()
//    {
//        Success = true
//    };

//    return response;

//    //if(!_rolesDisplay.Any(r => r.RoleName == roleName && r.IsSelected))
//    //{
//    //    _rolesDisplay.FirstOrDefault(r => r.RoleName == roleName).IsSelected = selectedPermission.IsSelected;
//    //}

//    //UserRoleDto? userRole = SelectedUser.Roles.FirstOrDefault(r => r.RoleName == roleName);
//    //if (userRole != null)
//    //{
//    //    if(!userRole.Permissions.Any(p => p == selectedPermission.PermissionName))
//    //    {
//    //        userRole.Permissions.Add(selectedPermission.PermissionName);
//    //    }
//    //    else
//    //    {
//    //        userRole.Permissions.Remove(selectedPermission.PermissionName);
//    //    }
//    //}
//    //else
//    //{
//    //    UserRoleDto newUserRole = new()
//    //    {
//    //        UserId = SelectedUser.Id,
//    //        RoleName = roleName,
//    //        Permissions = new List<string>() { selectedPermission.PermissionName }
//    //    };

//    //}
//}
}
