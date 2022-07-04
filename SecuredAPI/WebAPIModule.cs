using Autofac;
using Autofac.Core;

using Security.Core.Authorization.Handlers;
using Security.Core.Authorization.Providers;
using Security.Core.Permissions.Services;
using Security.Core.Services;

using Module = Autofac.Module;

namespace Security.Core;
public class WebAPIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<ToDoItemSearchService>().As<IToDoItemSearchService>().InstancePerLifetimeScope();


        //builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        //builder.Services.AddSingleton<IAuthorizationHandler, PermissionPolicyHandler>();
        //builder.Services.AddScoped<JwtTokenService, JwtTokenService>();
        builder.RegisterType<HashService>().As<IHashService>().InstancePerLifetimeScope();
        builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
        builder.RegisterType<RefreshTokenService>().As<IRefreshTokenService>().InstancePerLifetimeScope();
        builder.RegisterType<JwtTokenService>().As<IJwtTokenService>().InstancePerLifetimeScope();
        builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
        builder.RegisterType<AuthorizationPolicyProvider>().As<IAuthorizationPolicyProvider>().SingleInstance();
        builder.RegisterType<PermissionPolicyHandler>().As<IAuthorizationHandler>().SingleInstance();
      
    }
}
