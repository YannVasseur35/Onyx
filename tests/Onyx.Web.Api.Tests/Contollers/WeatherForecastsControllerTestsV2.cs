﻿namespace Onyx.Web.Api.Tests.Contollers
{
    public class WeatherForecastsControllerTestsV2 : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string baseEndPoint = "/api/weatherforecasts";

        private readonly WebApplicationFactory<Program> _factory;

        public WeatherForecastsControllerTestsV2(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_Ok()
        {
            using var _httpClient = _factory.CreateClient();

            //Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}