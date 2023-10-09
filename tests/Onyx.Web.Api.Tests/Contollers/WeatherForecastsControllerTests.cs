using Onyx.Application.Dtos;
using System;
using System.Collections.Generic;

namespace Onyx.Web.Api.Tests.Contollers
{
    public class WeatherForecastsControllerTests
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://localhost:7006") };
        private const string baseEndPoint = "/api/weatherforecasts";

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_Ok()
        {
            //Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}