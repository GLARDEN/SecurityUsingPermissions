using BlazorClient;
using BlazorClient.Handlers;
using BlazorClient.Providers;
using BlazorClient.Services;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Security.Shared.Authorization.Handlers;
using Security.Shared.Authorization.Providers;
using Security.Shared.Models.Administration.RoleManagement;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var newUri = new Uri("https://localhost:7047/");



builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = newUri)
                .AddHttpMessageHandler<AuthorizationMessageHandler>();


builder.Services.AddTransient<AuthorizationMessageHandler>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionDisplayService, PermissionDisplayService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

//Register the Permission policy handlers
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionPolicyHandler>();

builder.Services.AddScoped(typeof(IAppStateProvider<>), typeof(AppStateProvider<>));

// register the services
builder.Services.AddScoped<WeatherForecastService>();
await builder.Build().RunAsync();
