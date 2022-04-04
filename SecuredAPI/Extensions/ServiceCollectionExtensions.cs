using Security.Shared.Authorization.Handlers;
using Security.Shared.Authorization.Providers;

namespace SecuredAPI.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            });
        });

        services.AddAuthentication(options =>
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
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWTSettings:Secret"])),
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
        services.AddControllers().AddNewtonsoftJson();
    }

    internal static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
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
    }

    internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"), options =>
                {

                    options.MigrationsAssembly(typeof(AppDbContext).Assembly?.FullName?.Split(',')[0]);
                });
        });


        return services;
    }

    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //Register the Permission policy handlers
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionPolicyHandler>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IForecastService, ForecastService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IDatabaseCreator, DatabaseCreator>();
        return services;
    }


}
