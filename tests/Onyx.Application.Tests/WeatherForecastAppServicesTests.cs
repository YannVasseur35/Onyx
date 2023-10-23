using AutoMapper;
using Onyx.Application.Dtos;
using Onyx.Application.Interfaces;
using Onyx.Core.Interfaces;
using Onyx.Core.Models.Domain;
using Onyx.Infrastructure.Services;

namespace Onyx.Application.Tests
{
    public class WeatherForecastAppServicesTests
    {
        private readonly IWeatherForecastAppServices _weatherForecastAppServices;
        private readonly Mock<INotificationsAppServices> _notificationsServices;
        private readonly Mock<IWeatherForecastDataServices> _weatherForecastDataServices;
        private readonly Mock<IMapper> _mapper;

        public WeatherForecastAppServicesTests()
        {
            _notificationsServices = new Mock<INotificationsAppServices>();
            _weatherForecastDataServices = new Mock<IWeatherForecastDataServices>();
            _mapper = new Mock<IMapper>();

            _weatherForecastAppServices = new WeatherForecastAppServices(
                _notificationsServices.Object,
                _weatherForecastDataServices.Object,
                _mapper.Object
            );
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
        public async Task GetAllWeatherForecasts_ShouldReturn_2Items()
        {
            //Arrange
            IEnumerable<WeatherForecast>? mocks = new List<WeatherForecast> {
                new WeatherForecast() { City = "Paris", TemperatureC = 22},
                new WeatherForecast() { City = "Rennes", TemperatureC = 16},
            };

            _weatherForecastDataServices.Setup(x => x.GetAllAsync()).ReturnsAsync(mocks);

            //Act
            var results = await _weatherForecastAppServices.GetAllWeatherForecasts();

            //Assert
            Assert.NotNull(results);
            Assert.Equal<int>(mocks.Count(), results.Count());
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