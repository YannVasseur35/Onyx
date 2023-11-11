namespace Onyx.Web.Api.Tests.Contollers
{
    public class WeatherForecastsControllerTests : IClassFixture<AppTestWebApplicationFactory>
    {
        private const string baseEndPoint = "/api/WeatherForecasts";
        private readonly AppTestWebApplicationFactory _webAppFactory;
        private readonly HttpClient _httpClient;
        private readonly Fixture _fixture;

        public WeatherForecastsControllerTests(
            AppTestWebApplicationFactory webAppFactory)
        {
            _webAppFactory = webAppFactory;
            _httpClient = webAppFactory.CreateClient();
            _fixture = new Fixture();
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

        [Fact]
        public async Task GetAsync_ShouldReturn_Ok()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var operation = _fixture.Create<OperationResult<WeatherForecastDto?>>();
                    operation.IsOperationSuccess = true;
                    if (operation.Model != null)
                    {
                        operation.Model.Id = id;
                    }

                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(operation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var response = await client.GetAsync($"{baseEndPoint}/{id}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            string json = await response.Content.ReadAsStringAsync();
            WeatherForecastDto? data = JsonSerializer.Deserialize<WeatherForecastDto>(json, GlobalConsts.JsonSerializerOptions);
            Assert.Equal(id, data?.Id);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NoContent()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var expectedStatusCode = System.Net.HttpStatusCode.NoContent;

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}/{id}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_BadRequest_OnBadArguments()
        {
            //Arrange

            //Act
            var response = await _httpClient.GetAsync($"{baseEndPoint}/prout");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_BadRequest_OnBadOperation()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var expectedStatusCode = System.Net.HttpStatusCode.BadRequest;

            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var operation = _fixture.Create<OperationResult<WeatherForecastDto?>>();
                    operation.IsOperationSuccess = false;
                    operation.Model = null;

                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(operation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var response = await client.GetAsync($"{baseEndPoint}/{id}");

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_Ok()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var operation = _fixture.Create<Operation>();
                    operation.IsOperationSuccess = true;

                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(operation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var response = await client.DeleteAsync($"{baseEndPoint}/{id}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_NoContent()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var operation = _fixture.Create<Operation>();
                    operation.IsOperationSuccess = false;

                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(operation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var response = await client.DeleteAsync($"{baseEndPoint}/{id}");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_BadRequest_OnBadArguments()
        {
            //Arrange

            //Act
            var response = await _httpClient.DeleteAsync($"{baseEndPoint}/prout");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ShouldReturn_Ok_And_Guid()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var weatherForecastDto = _fixture.Create<WeatherForecastDto>();
            weatherForecastDto.Id = id;

            var mokedOperation = _fixture.Create<OperationResult<Guid>>();
            mokedOperation.IsOperationSuccess = true;

            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.SaveAsync(It.IsAny<WeatherForecastDto>())).ReturnsAsync(mokedOperation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var stringContent = new StringContent(JsonSerializer.Serialize(weatherForecastDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseEndPoint}/", stringContent);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            string guidStr = await response.Content.ReadAsStringAsync();
            Guid guid = Guid.Parse(guidStr.Trim('\"'));
            Assert.True(guid != Guid.Empty);
            Assert.Equal(mokedOperation.Model, guid);
        }

        [Fact]
        public async Task PostAsync_ShouldReturn_BadRequest_OnBadArguments()
        {
            //Arrange

            //Act
            var response = await _httpClient.PostAsync($"{baseEndPoint}/", new StringContent("", Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ShouldReturn_BadRequest_OnBadOperation()
        {
            //Arrange
            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var operation = _fixture.Create<OperationResult<Guid>>();
                    operation.IsOperationSuccess = false;

                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.SaveAsync(It.IsAny<WeatherForecastDto>())).ReturnsAsync(operation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var stringContent = new StringContent(JsonSerializer.Serialize(_fixture.Create<WeatherForecastDto>()), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseEndPoint}/", stringContent);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutAsync_ShouldReturn_Ok()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var weatherForecastDto = _fixture.Create<WeatherForecastDto>();
            weatherForecastDto.Id = id;
            weatherForecastDto.Summary = "something";

            var client = _webAppFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var operation = _fixture.Create<OperationResult<Guid>>();
                    operation.IsOperationSuccess = true;

                    var weatherForecastAppServicesMocked = new Mock<IWeatherForecastAppServices>();
                    weatherForecastAppServicesMocked.Setup(x => x.SaveAsync(It.IsAny<WeatherForecastDto>())).ReturnsAsync(operation);
                    services.Replace(ServiceDescriptor.Scoped(typeof(IWeatherForecastAppServices), _ => weatherForecastAppServicesMocked.Object));
                });
            }).CreateClient();

            //Act
            var stringContent = new StringContent(JsonSerializer.Serialize(weatherForecastDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseEndPoint}/", stringContent);

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}