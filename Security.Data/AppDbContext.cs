using Microsoft.EntityFrameworkCore;

using Security.Shared.Models;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.UserManagement;

using System.Reflection.Emit;

namespace Security.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Role> Roles{ get; set; } = null!;
    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        builder.Entity<User>().HasKey(k => k.Id);


        builder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleName });       
        


        builder.Entity<Role>()  
               .HasIndex(x => x.RoleName)
               .IsUnique();

        builder.Entity<Role>().HasKey(x => x.RoleName);
        builder.Entity<Role>().Property<string>("_permissionsInRole")
            .HasColumnName("PermissionsInRole");

        base.OnModelCreating(builder);
    }

}