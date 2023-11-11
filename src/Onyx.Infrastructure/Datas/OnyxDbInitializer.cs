using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Onyx.Core.Models.Domain;
using Onyx.Infrastructure.Models.Entities;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Onyx.Infrastructure.Datas
{
    [ExcludeFromCodeCoverage]
    public static class OnyxDbInitializer
    {
        public static void CreateDbIfNotExists(IServiceScope scope, ILogger logger)
        {
            IServiceProvider? services = scope.ServiceProvider;

            if (services != null)
            {
                try
                {
                    var context = services.GetRequiredService<OnyxDbContext>();

                    Initialize(context, logger);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
            else
            {
                logger.LogError("CreateDbIfNotExists error, service is null");
            }
        }

        public static void Initialize(OnyxDbContext context, ILogger logger)
        {
            try
            {
                //Important, apply all migrations before init. Usefull on staging/production
                context.Database.Migrate();

                var dbCreated = context.Database.EnsureCreated();

                logger.LogInformation("dbCreated : " + dbCreated);

                // Look for any WeatherForecasts.
                if (context.WeatherForecasts != null)
                {
                    if (context.WeatherForecasts.Any())
                    {
                        return;   // DB has been seeded
                    }

                    var weatherForecasts = new List<WeatherForecastEntity>()
                    {
                         new WeatherForecastEntity(){ City = "Paris", TemperatureC=23},
                         new WeatherForecastEntity(){ City = "Rennes", TemperatureC=22},
                    };

                    context.WeatherForecasts.AddRange(weatherForecasts);

                    context.SaveChanges();
                }
                else
                {
                    logger.LogError("Initialize DB error : WeatherForecasts is null");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Initialize DB error : " + ex.StackTrace);
            }
        }
    }
}