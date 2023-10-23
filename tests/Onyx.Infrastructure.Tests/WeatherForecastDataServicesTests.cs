using Onyx.Core.Interfaces;
using Onyx.Infrastructure.Services;

namespace Onyx.Infrastructure.Tests
{
    public class WeatherForecastDataServicesTests
    {
        private readonly IWeatherForecastDataServices _weatherForecastDataServices;

        public WeatherForecastDataServicesTests()
        {
            _weatherForecastDataServices = new WeatherForecastDataServices();
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_NoNull()
        {
            //Arrange

            //Act
            var results = await _weatherForecastDataServices.GetAllAsync();

            //Assert
            Assert.NotNull(results);
        }
    }
}