using Autofac;
using Autofac.Extensions.DependencyInjection;

using MediatR;

using Security.Core;
using Security.Core.Authorization.Handlers;
using Security.Core.Authorization.Providers;
using Security.Core.Events;
using Security.Core.Services;
using Security.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

string connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

var assemblies = new Assembly[]
{
    typeof(Program).Assembly,
    typeof(CoreModule).Assembly,
    typeof(InfrastructureModule).Assembly
};

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new CoreModule());
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //TODO - Lowercase Swagger Documents
    //c.DocumentFilter<LowercaseDocumentFilter>();
    //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f

    // include all project's xml comments
    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
        if (!assembly.IsDynamic)
        {
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(baseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        }
    }

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SecuuredAPI",
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
   });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(assemblies);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionPolicyHandler>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IDatabaseCreator, DatabaseCreator>();
builder.Services.AddDbContext(connectionString);

builder.Services.AddControllers().AddNewtonsoftJson();



//ServiceLocator.SetLocatorProvider(builder.Services.BuildServiceProvider());
//DomainEvents.Mediator = () => ServiceLocator.Current.GetInstance<IMediator>();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    // options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
    options.RoutePrefix = "swagger";
    options.DisplayRequestDuration();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

EnsureDataStorageIsReady(app, builder.Configuration);



app.Run();


void EnsureDataStorageIsReady(IHost app, IConfiguration configuration)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory?.CreateScope();

    try
    {
        var createAndSeedDB = configuration.GetValue<bool>("appSettings:createAndSeedDB");
        if (createAndSeedDB)
        {
            var appDbContext = scope?.ServiceProvider.GetService<AppDbContext>();
            if (appDbContext != null)
            {
                appDbContext.Database.EnsureDeletedAsync().Wait();
                appDbContext.Database.EnsureCreatedAsync().Wait();

                var databaseCreator = scope?.ServiceProvider.GetService<IDatabaseCreator>();

                databaseCreator?.Initialize();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = scope?.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger?.LogError(ex, "An error occurred while migrating or seeding the database.");

        throw;
    }

}
