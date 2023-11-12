using Onyx.Core;
using Onyx.Web.Api.Integration.Tests;
using Onyx.Application.Dtos;

namespace Onyx.Web.Api.Integration.Tests
{
    [Collection("WeatherForecasts tests")]
    public class WeatherForecastsControllerTests : IClassFixture<AppTestWebApplicationFactory>
    {
        private const string baseEndPoint = "/api/WeatherForecasts";

        private readonly HttpClient _httpClient;
        private readonly Fixture _fixture;

        private readonly WeatherForecastDto _weatherForecastDto;

        public WeatherForecastsControllerTests(
            AppTestWebApplicationFactory webAppFactory)
        {
            _httpClient = webAppFactory.CreateClient();
            _fixture = new Fixture();

            _weatherForecastDto = _fixture.Create<WeatherForecastDto>();
            _weatherForecastDto.Id = Guid.Empty;
        }

        [Fact]
        public async Task SequenceCRUD()
        {
            await CRUD_1_Get();
            await CRUD_2_Post();
            await CRUD_3_Get();
            await CRUD_4_Put();
            await CRUD_5_Get();
            await CRUD_6_Delete();
            await CRUD_7_Get();
        }

        private async Task CRUD_1_Get()
        {
            //Arrange

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}/{_weatherForecastDto.Id}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        private async Task CRUD_2_Post()
        {
            //Arrange

            //Act
            var stringContent = new StringContent(JsonSerializer.Serialize(_weatherForecastDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{baseEndPoint}/", stringContent);
            string guidStr = await response.Content.ReadAsStringAsync();
            Guid guid = Guid.Parse(guidStr.Trim('\"'));
            _weatherForecastDto.Id = guid;

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.True(Guid.Empty != guid);
        }

        private async Task CRUD_3_Get()
        {
            //Arrange

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}/{_weatherForecastDto.Id}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string json = await response.Content.ReadAsStringAsync();
            WeatherForecastDto? data = JsonSerializer.Deserialize<WeatherForecastDto>(json, GlobalConsts.JsonSerializerOptions);
            Assert.Equal(_weatherForecastDto.Id, data?.Id);

            //Assert.Equal(_weatherForecastDto, data);
        }

        private async Task CRUD_4_Put()
        {
            //Arrange
            _weatherForecastDto.Summary = "something";

            //Act
            var stringContent = new StringContent(JsonSerializer.Serialize(_weatherForecastDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseEndPoint}/", stringContent);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        private async Task CRUD_5_Get()
        {
            //Arrange

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}/{_weatherForecastDto.Id}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string json = await response.Content.ReadAsStringAsync();
            WeatherForecastDto? data = JsonSerializer.Deserialize<WeatherForecastDto>(json, GlobalConsts.JsonSerializerOptions);
            Assert.Equal(_weatherForecastDto.Summary, data?.Summary);
        }

        private async Task CRUD_6_Delete()
        {
            //Arrange

            //Act
            var response = await _httpClient.DeleteAsync($"{baseEndPoint}/{_weatherForecastDto.Id}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        private async Task CRUD_7_Get()
        {
            //Arrange
            string deleteId = _weatherForecastDto.Id.ToString() ?? "";

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}/{deleteId}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}