using AutoMapper;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions.Extensions;

namespace SecuredAPI;

public class AutoMapping :Profile
{
    public AutoMapping()
    {
        CreateMap<Forecast, WeatherForecastDto>();

        CreateMap<Role, RoleDto>()
            .ForMember(dto => dto.PermissionsInRole,
            opt => opt.MapFrom(r => r.PermissionsInRole.ConvertPackedPermissionToNames()));


        CreateMap<RefreshToken, RefreshTokenDto>();
        CreateMap<UserDto, User>()
         .ForMember(dto => dto.RefreshTokens, c => c.MapFrom(u => u.RefreshTokens));

        CreateMap<User, UserDto>()
              .ForMember(dto=> dto.AssignedRoles, c => c.MapFrom(u => u.UserRoles));

        CreateMap<UserRole, UserRoleDto>()
            .ForMember(dto => dto.AssignedPermissions, option => option.MapFrom(u => u.AssignedPermissions.ConvertPackedPermissionToNames()));
        CreateMap<UserRoleDto, UserRole>()
          .ForMember(u => u.AssignedPermissions, option => option.MapFrom(u => u.AssignedPermissions.PackPermissionsNames()));
    }
}
