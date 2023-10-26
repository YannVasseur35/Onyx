using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.Infrastructure.Models.Entities;

namespace Onyx.Infrastructure.Datas
{
    public class OnyxDbContext : DbContext
    {
        public OnyxDbContext(DbContextOptions<OnyxDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }
    }
}