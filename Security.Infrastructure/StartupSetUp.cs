using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Infrastructure;
public static class StartupSetUp
{

    public static void AddDbContext(this IServiceCollection services, string connectionString) 
    {
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(connectionString);
        });
    }
}
