namespace Security.Infrastructure;
public class DatabaseCreator : IDatabaseCreator
{
    private readonly AppDbContext _appDbContext;

    public object PermissionsDisplay { get; private set; }

    public DatabaseCreator(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Initialize()
    {
        CreateDefaultRoles();
        CreateForecasts();
        CreateDefaultUsers();
    }

    private void CreateDefaultRoles()
    {
        Task.Run(async () =>
        {
            await _appDbContext.Roles.AddRangeAsync(SecuritySetUpData.DefaultRoleDefinitions);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateForecasts()
    {
        Task.Run(async () =>
        {
            await _appDbContext.WeatherForecasts.AddRangeAsync(SecuritySetUpData.GetSampleForecastData());
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateDefaultUsers()
    {
        Task.Run(async () =>
        {
            await _appDbContext.Users.AddRangeAsync(SecuritySetUpData.GetDefaultUsers());
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

}

