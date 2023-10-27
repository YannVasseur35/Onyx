using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Onyx.Core.Interfaces;
using Onyx.Core.Models.Domain;
using Onyx.Infrastructure.Datas;
using Onyx.Infrastructure.Models.Entities;
using Onyx.Infrastructure.Services;

namespace Onyx.Infrastructure.Tests
{
    public class WeatherForecastDataServicesTests
    {
        private readonly IMapper _mapper;

        private DbContextOptions<OnyxDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<OnyxDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        public WeatherForecastDataServicesTests()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<Mappings.MappingProfiles>();
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_Data()
        {
            //Arrange
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                context.WeatherForecasts.Add(new WeatherForecastEntity { City = "Rennes", TemperatureC = 24 });
                context.SaveChanges();

                var weatherForecastDataServices = new WeatherForecastDataServices(context, _mapper);
                var results = await weatherForecastDataServices.GetAllAsync();

                Assert.NotNull(results);
                Assert.True(results.Count() > 0);
                Assert.True(results.FirstOrDefault()?.City == "Rennes");
                Assert.True(results.FirstOrDefault()?.TemperatureC == 24);
            }
        }
    }
}