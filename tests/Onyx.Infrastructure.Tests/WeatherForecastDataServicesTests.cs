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
        private readonly Mock<IMapper> _mapper;

        private DbContextOptions<OnyxDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<OnyxDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        public WeatherForecastDataServicesTests()
        {
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_NoNull()
        {
            //Arrange
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                context.Add(new WeatherForecastEntity { City = "Rennes", TemperatureC = 24 });
                context.SaveChanges();
            }

            ///Act and Assert
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                var weatherForecastDataServices = new WeatherForecastDataServices(context, _mapper.Object);
                var results = await weatherForecastDataServices.GetAllAsync();

                Assert.NotNull(results);
                Assert.True(results.Count() > 0);
            }
        }
    }
}