using Onyx.Application.Dtos;
using Onyx.Application.Interfaces;

namespace Onyx.Application.Tests
{
    public class WeatherForecastAppServicesTests
    {
        private readonly IWeatherForecastAppServices _weatherForecastAppServices;
        private readonly Mock<INotificationsAppServices> _notificationsServices;

        public WeatherForecastAppServicesTests()
        {
            _notificationsServices = new Mock<INotificationsAppServices>();
            _weatherForecastAppServices = new WeatherForecastAppServices(_notificationsServices.Object);
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_NoNull()
        {
            //Arrange

            //Act
            var results = await _weatherForecastAppServices.GetAllWeatherForecasts();

            //Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task CreateWeatherForecasts_Should_CallNotificationService_WhenFreezing()
        {
            //Arrange
            var mock = new Mock<WeatherForecastDto>();
            mock.Object.TemperatureC = 0;

            //Act
            var operation = await _weatherForecastAppServices.CreateWeatherForecasts(mock.Object);

            //Assert
            Assert.True(operation.IsOperationSuccess);
            _notificationsServices.Verify(x => x.WeatherAlertAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
        }
    }
}