using Microsoft.EntityFrameworkCore;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;
using Security.Core.Models.WeatherForecast;


namespace Security.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Forecast> WeatherForecasts { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        builder.Entity<User>().HasKey(k => k.Id);

        builder.Entity<RefreshToken>().HasKey(k => new {k.UserId, k.DeviceId });      
        builder.Entity<RefreshToken>().HasIndex(i => new { i.UserId, i.DeviceId, i.IsInvalid}).IsUnique();
        builder.Entity<RefreshToken>().HasQueryFilter(rt => !rt.IsInvalid);

        builder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleName });

        builder.Entity<Role>()
               .HasIndex(x => x.Name)
               .IsUnique();

        builder.Entity<Role>().HasKey(x => x.Id);

        base.OnModelCreating(builder);
    }

}