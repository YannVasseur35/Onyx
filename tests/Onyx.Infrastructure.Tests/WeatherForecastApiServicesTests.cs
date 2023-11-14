using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Onyx.Core.Models.OpenWeatherMap;
using System.Text;

namespace Onyx.Infrastructure.Tests
{
    public class WeatherForecastApiServicesTests
    {
        private readonly IWeatherForecastApiServices _weatherForecastApiServices;

        public WeatherForecastApiServicesTests()
        {
            var builder = new ConfigurationBuilder();

            var appSettings = @"{
                ""OpenWeatherMapApiBaseUrl"" : ""https://api.openweathermap.org/data/2.5/weather"",
                ""OpenWeatherMapApiKey"" : ""2ab3de3a263b8b265877be0c4acf25b3""
            }";
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

            var configuration = builder.Build();

            HttpClient _httpClient = new HttpClient();
            var openWeatherMapHttpClient = new OpenWeatherMapHttpClient(_httpClient, configuration);

            _weatherForecastApiServices = new WeatherForecastApiServices(openWeatherMapHttpClient, configuration);
        }

        [Fact]
        public async Task GetMonitorsAsync_ShouldReturn_NotNull()
        {
            //Arrange

            //Act
            OpenWeatherMapResponseDto response = await _weatherForecastApiServices.GetForecastAsync("Rennes");

            //Assert
            Assert.NotNull(response);
            Assert.True(response.main?.temp > -50);
        }
    }
}