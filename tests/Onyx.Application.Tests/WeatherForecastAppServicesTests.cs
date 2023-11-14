namespace Onyx.Application.Tests
{
    public class WeatherForecastAppServicesTests
    {
        private readonly IWeatherForecastAppServices _weatherForecastAppServices;
        private readonly Mock<INotificationsAppServices> _notificationsServices;
        private readonly Mock<IWeatherForecastDataServices> _weatherForecastDataServices;
        private readonly Mock<IWeatherForecastApiServices> _weatherForecastApiServices;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public WeatherForecastAppServicesTests()
        {
            _notificationsServices = new Mock<INotificationsAppServices>();
            _weatherForecastDataServices = new Mock<IWeatherForecastDataServices>();
            _weatherForecastApiServices = new Mock<IWeatherForecastApiServices>();

            _fixture = new Fixture();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfiles>();
            });

            _mapper = config.CreateMapper();

            _weatherForecastAppServices = new WeatherForecastAppServices(
                _notificationsServices.Object,
                _weatherForecastDataServices.Object,
                _weatherForecastApiServices.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturn_NoNull()
        {
            //Arrange

            //Act
            var results = await _weatherForecastAppServices.GetAllAsync();

            //Assert
            Assert.NotNull(results);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturn_Null()
        {
            //Arrange
            _weatherForecastDataServices.Setup(x => x.GetAllAsync()).ReturnsAsync(() => null);

            //Act
            var results = await _weatherForecastAppServices.GetAllAsync();

            //Assert
            Assert.Null(results);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturn_2Items()
        {
            //Arrange

            IEnumerable<WeatherForecast>? mocks = new List<WeatherForecast> {
                _fixture.Create<WeatherForecast>(),
                _fixture.Create<WeatherForecast>()
            };

            _weatherForecastDataServices.Setup(x => x.GetAllAsync()).ReturnsAsync(mocks);

            //Act
            var results = await _weatherForecastAppServices.GetAllAsync();

            //Assert
            Assert.NotNull(results);
            Assert.Equal(mocks.Count(), results.Count());
        }

        /**********/

        [Fact]
        public async Task GetByIdAsync_ShouldReturn_Item()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            var moq = _fixture.Create<WeatherForecast>();
            moq.Id = id;

            _weatherForecastDataServices.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(moq);

            //Act
            var operation = await _weatherForecastAppServices.GetByIdAsync(id);

            //Assert
            Assert.NotNull(operation);
            Assert.True(operation.IsOperationSuccess);
            Assert.NotNull(operation.Model);
            Assert.Equal(moq.Summary, operation.Model.Summary);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturn_Null()
        {
            //Arrange
            _weatherForecastDataServices.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((WeatherForecast?)null);

            //Act
            var operation = await _weatherForecastAppServices.GetByIdAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(operation);
            Assert.True(operation.IsOperationSuccess); //C'est un succès malgrès le fait que l'on est null en retour.
            Assert.Null(operation.Model);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturn_OperationFailure_WhenIdNullOrEmpty()
        {
            //Arrange
            _weatherForecastDataServices.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync((WeatherForecast?)null);

            //Act
            var operation = await _weatherForecastAppServices.GetByIdAsync(Guid.Empty);

            //Assert
            Assert.NotNull(operation);
            Assert.False(operation.IsOperationSuccess);
            Assert.Null(operation.Model);
            Assert.NotNull(operation.ErrorMessage);
            Assert.True(operation.ErrorMessage?.Length > 0);
            Assert.True(operation.ErrorType == OperationErrorType.Functional);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturn_OperationFailure_WhenException()
        {
            //Arrange
            _weatherForecastDataServices.Setup(x => x.GetAsync(It.IsAny<Guid>())).Throws(new Exception("Test Message"));

            //Act
            var operation = await _weatherForecastAppServices.GetByIdAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(operation);
            Assert.False(operation.IsOperationSuccess);
            Assert.Null(operation.Model);
            Assert.NotNull(operation.ErrorMessage);
            Assert.Contains("Test Message", operation.ErrorMessage);
            Assert.True(operation.ErrorMessage?.Length > 0);
            Assert.True(operation.ErrorType == OperationErrorType.Technical);
        }

        /**********/

        [Fact]
        public async Task SaveAsync_Should_ReturnOperationSuccess()
        {
            //Arrange
            var newItem = _fixture.Create<WeatherForecastDto>();

            //Act
            var operation = await _weatherForecastAppServices.SaveAsync(newItem);

            //Assert
            Assert.NotNull(operation);
            Assert.True(operation.IsOperationSuccess);
            Assert.Null(operation.ErrorMessage);
        }

        [Fact]
        public async Task SaveAsync_ShouldReturn_OperationFailure_WhenException()
        {
            //Arrange
            _weatherForecastDataServices.Setup(x => x.AddOrUpdateAsync(It.IsAny<WeatherForecast>())).Throws(new Exception("Test Message"));

            //Act
            var operation = await _weatherForecastAppServices.SaveAsync(It.IsAny<WeatherForecastDto>());

            //Assert
            Assert.NotNull(operation);
            Assert.False(operation.IsOperationSuccess);
            Assert.NotNull(operation.ErrorMessage);
            Assert.Contains("Test Message", operation.ErrorMessage);
            Assert.True(operation.ErrorMessage?.Length > 0);
            Assert.True(operation.ErrorType == OperationErrorType.Technical);
        }

        /**********/

        [Fact]
        public async Task DeleteAsync_Should_ReturnOperationSuccess()
        {
            //Arrange
            var newItem = _fixture.Create<WeatherForecastDto>();

            //Act
            var operation = await _weatherForecastAppServices.DeleteAsync(newItem.Id);

            //Assert
            Assert.NotNull(operation);
            Assert.True(operation.IsOperationSuccess);
            Assert.Null(operation.ErrorMessage);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_OperationFailure_WhenException()
        {
            //Arrange
            _weatherForecastDataServices.Setup(x => x.DeleteAsync(It.IsAny<Guid>())).Throws(new Exception("Test Message"));

            //Act
            var operation = await _weatherForecastAppServices.DeleteAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(operation);
            Assert.False(operation.IsOperationSuccess);
            Assert.NotNull(operation.ErrorMessage);
            Assert.Contains("Test Message", operation.ErrorMessage);
            Assert.True(operation.ErrorMessage?.Length > 0);
            Assert.True(operation.ErrorType == OperationErrorType.Technical);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturn_OperationFailure_WhenIdNullOrEmpty()
        {
            //Arrange

            //Act
            var operation = await _weatherForecastAppServices.DeleteAsync(Guid.Empty);

            //Assert
            Assert.NotNull(operation);
            Assert.False(operation.IsOperationSuccess);
            Assert.NotNull(operation.ErrorMessage);
            Assert.NotEmpty(operation.ErrorMessage);
            Assert.True(operation.ErrorType == OperationErrorType.Functional);
        }
    }
}