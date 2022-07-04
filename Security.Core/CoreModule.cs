using Autofac;

using Security.Core.Models.Administration.RoleManagement.Services;
using Security.Core.Models.UserManagement.Services;
using Security.Core.Services;
using Security.Core.Services.WeatherForecast;

namespace Security.Core;
public class CoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<IJwtTokenService, JwtTokenService>();
        //builder.RegisterType<IAuthenticationService, AuthenticationService>();
        //builder.RegisterType<IProfileService, ProfileService>();
        //builder.RegisterType<IForecastService, ForecastService>();
        //builder.RegisterType<IRoleService, RoleService>();
        //builder.RegisterType<IUserManagementService, UserManagementService>();
        //builder.RegisterType<IDatabaseCreator, DatabaseCreator>();


        //builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();        
        builder.RegisterType<HashService>().As<IHashService>().InstancePerLifetimeScope();
        builder.RegisterType<ForecastService>().As<IForecastService>().InstancePerLifetimeScope();
        builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
        builder.RegisterType<UserManagementService>().As<IUserManagementService>().InstancePerLifetimeScope();





       
    }
}
