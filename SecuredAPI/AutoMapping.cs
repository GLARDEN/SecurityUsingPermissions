using AutoMapper;

using Security.Shared.Models;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Extensions;

namespace SecuredAPI;

public class AutoMapping :Profile
{
    public AutoMapping()
    {
        CreateMap<WeatherForecast, WeatherForecastDto>();
        CreateMap<User, UserDto>();
        CreateMap<UserRole, UserRoleDto>();
           // .ForMember(dto => dto.Permissions,
          //  opt => opt.MapFrom(ur => ur.AssignedPermissions.ConvertPackedPermissionToNames()));

        CreateMap<Role, RoleDto>();
    }
}
