namespace Onyx.Web.Api.Tests.Contollers
{
    public class WeatherForecastsControllerTests : IClassFixture<AppTestFixture>
    {
        private readonly HttpClient _httpClient;
        private const string baseEndPoint = "/api/weatherforecasts";

        public WeatherForecastsControllerTests(AppTestFixture fixture)
        {
            _httpClient = fixture.CreateClient();
        }

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