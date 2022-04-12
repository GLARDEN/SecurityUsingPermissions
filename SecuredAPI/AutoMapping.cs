using AutoMapper;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.UserManagement;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions.Extensions;

namespace SecuredAPI;

public class AutoMapping :Profile
{
    public AutoMapping()
    {
        CreateMap<Forecast, WeatherForecastDto>();
        CreateMap<User, UserDto>();
        CreateMap<UserRole, UserRoleDto>();
           // .ForMember(dto => dto.Permissions,
          //  opt => opt.MapFrom(ur => ur.AssignedPermissions.ConvertPackedPermissionToNames()));

        CreateMap<Role, RoleDto>()
            .ForMember(dto => dto.PermissionsInRole,
            opt => opt.MapFrom(r => r.PermissionsInRole.ConvertPackedPermissionToNames()));
    }
}
