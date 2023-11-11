using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Onyx.Infrastructure.Models.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Onyx.Infrastructure.Datas
{
    [ExcludeFromCodeCoverage]
    public class OnyxDbContext : DbContext
    {
        public OnyxDbContext(DbContextOptions<OnyxDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }
    }
}