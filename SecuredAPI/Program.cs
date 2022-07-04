
using Autofac;
using Autofac.Extensions.DependencyInjection;

using MediatR;

using Security.Core;
using Security.Infrastructure;
using Security.SharedKernel;
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

string connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

builder.Services.AddDbContext(connectionString);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(option =>
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
                option.IncludeXmlComments(xmlPath);
            }
        }
    }
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SecuuredAPI",
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
     {
           new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
     }
 });
});

var assemblies = new Assembly[]
{
    typeof(SharedKernelModule).Assembly,
    typeof(CoreModule).Assembly,
    typeof(InfrastructureModule).Assembly,
    typeof(WebAPIModule).Assembly

};
builder.Services.AddAutoMapper(assemblies);

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new WebAPIModule());
    containerBuilder.RegisterModule(new SharedKernelModule());
    containerBuilder.RegisterModule(new CoreModule());
    containerBuilder.RegisterModule(new InfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

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

            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtAuth:Issuer"],
            ValidAudience = builder.Configuration["JwtAuth:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Secret"])),
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



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );

app.UseRouting();
app.UseHttpsRedirection();



//app.AddCors(options =>
//{
//    options.AddPolicy("Policy1",
//        builder =>
//        {
//            builder.WithOrigins("http://example.com",
//                                "http://www.contoso.com");
//        });

//    options.AddPolicy("AllowedClientPolicy",
//        builder =>
//        {
//            builder.WithOrigins($"https://{host}:7047/",
//                                $"http://{host}:5000",   // gglwa
//                                $"https://{host}:5001",  // gglwa
//                                $"http://{host}:5004",   // gglsv
//                                $"https://{host}:5005",  // gglsv
//                                $"https://{host}:44374", // gglwa
//                                $"https://{host}:44370", // gglsv
//                                $"http://localhost:5000",   // gglwa
//                                $"https://localhost:5001",  // gglwa
//                                $"http://localhost:5004",   // gglsv
//                                $"https://localhost:5005",  // gglsv
//                                $"https://localhost:44374", // gglwa
//                                $"https://localhost:44370") // gglsv
//                                .AllowAnyHeader()
//                                .AllowAnyMethod();
//        });
//});

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
    options.RoutePrefix = "swagger";

    options.DisplayRequestDuration();
});

SetUpMediatR(app);

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


void SetUpMediatR(IApplicationBuilder app)
{
    var serviceProvider = app.ApplicationServices;
    DomainEvents.Mediator = () => BuildMediator(serviceProvider);
}

IMediator BuildMediator(IServiceProvider serviceProvider)
{
    return serviceProvider.GetRequiredService<IMediator>();
}
