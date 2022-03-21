using SecuredAPI.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigureContainer(builder);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureApp(app, builder.Environment);

EnsureDataStorageIsReady(app, builder.Configuration);

app.Run();

void ConfigureContainer(WebApplicationBuilder builder)
{

}

void ConfigureApp(WebApplication app, IWebHostEnvironment env)
{
    app.UseExceptionHandling(builder.Environment);
    app.UseHttpsRedirection();

    app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints();
    app.ConfigureSwagger();
}

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddDatabase(configuration);
    services.AddMemoryCache();
    services.AddApplicationLayer(configuration);
    services.AddApplicationServices();        
    services.AddEndpointsApiExplorer();
    services.RegisterSwagger();
}

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
