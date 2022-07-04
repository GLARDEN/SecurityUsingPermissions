using BlazorClient;
using BlazorClient.Handlers;
using BlazorClient.Interfaces;
using BlazorClient.Providers;
using BlazorClient.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Security.Core.Authorization.Handlers;
using Security.Core.Authorization.Providers;
using Security.Core.Permissions.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var newUri = new Uri("https://localhost:7047/");

builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.AddHttpClient("api",httpClient =>
{
    httpClient.BaseAddress = newUri;
}).AddHttpMessageHandler<RefreshTokenHandler>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IAuthenticationUiService, AuthenticationUiService>();
builder.Services.AddScoped<IRefreshTokenUiService, RefreshTokenUiService>();
builder.Services.AddScoped<IUserManagementUiService, UserManagementUiService>();
builder.Services.AddScoped<IRoleUiService, RoleUiService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

//Register the Permission policy handlers
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionPolicyHandler>();

builder.Services.AddScoped(typeof(IAppStateProvider<>), typeof(AppStateProvider<>));

// register the services
builder.Services.AddScoped<WeatherForecastUiService>();

await builder.Build().RunAsync();
