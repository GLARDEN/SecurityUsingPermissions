using Microsoft.EntityFrameworkCore;

using Security.Shared.Models;

using System.Reflection.Emit;

namespace Security.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;
    public DbSet<RoleToPermissions> RolesToPermissions { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
{
        builder.Entity<UserToRole>().HasKey(x => new { x.UserId, x.RoleName });

        builder.Entity<RoleToPermissions>().HasKey(x => x.RoleName);
        builder.Entity<RoleToPermissions>().Property<string>("_permissionsInRole")
            .HasColumnName("PermissionsInRole");

        base.OnModelCreating(builder);
    }

}