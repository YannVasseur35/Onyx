using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.Infrastructure.Models.Entities;

namespace Onyx.Infrastructure.Datas
{
    public class OnyxDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public OnyxDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                 x => x.MigrationsAssembly("Onyx.Infrastructure"));
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }
    }
}