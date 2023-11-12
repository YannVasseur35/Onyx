namespace Onyx.Infrastructure.Tests
{
    public class WeatherForecastDataServicesTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        private DbContextOptions<OnyxDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<OnyxDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase{Guid.NewGuid().ToString()}")
                .Options;
        }

        public WeatherForecastDataServicesTests()
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<Mappings.MappingProfiles>();
            });

            _mapper = config.CreateMapper();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllWeatherForecasts_ShouldReturn_Data()
        {
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                //Arrange
                context.WeatherForecasts.Add(new WeatherForecastEntity { Summary = "DDCDE" });
                context.SaveChanges();
                var service = new WeatherForecastDataServices(context, _mapper);

                //Act
                var results = await service.GetAllAsync();

                //Assert
                Assert.NotNull(results);
                Assert.True(results.Any());
                Assert.True(results.FirstOrDefault()?.Summary == "DDCDE");
            }
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Data()
        {
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                //Arrange
                var weatherForecastEntity = new WeatherForecastEntity
                {
                    Summary = "DDCDE"
                };
                context.WeatherForecasts.Add(weatherForecastEntity);
                context.SaveChanges();
                var service = new WeatherForecastDataServices(context, _mapper);

                //Act
                var result = await service.GetAsync(weatherForecastEntity.Id);

                //Assert
                Assert.NotNull(result);
                Assert.True(result.Summary == "DDCDE");
            }
        }

        [Fact]
        public async Task AddOrUpdateAsync_Should_AddData()
        {
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                //Arrange
                var weatherForecast = _fixture.Create<WeatherForecast>();
                weatherForecast.Id = Guid.Empty; //New item

                var service = new WeatherForecastDataServices(context, _mapper);

                //Act
                var guid = await service.AddOrUpdateAsync(weatherForecast);

                //Assert
                Assert.NotEqual(Guid.Empty, guid);
                var result = context.WeatherForecasts.Find(guid);
                Assert.NotNull(result);
                Assert.True(result.Summary == weatherForecast.Summary);
                Assert.True(result.Id != Guid.Empty);
                Assert.True(result.CreatedAt == result.ModifiedAt);
                Assert.True(result.ModifiedAt > DateTime.UtcNow.AddSeconds(-5));
            }
        }

        [Fact]
        public async Task AddOrUpdateAsync_Should_UpdateData()
        {
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                //Arrange
                DateTime createdAt = DateTime.UtcNow.AddDays(-5);
                Guid id = Guid.NewGuid();

                _fixture.Customize<WeatherForecastEntity>(c => c.Do(o => o.Init(id, createdAt)));
                var entityInDb = _fixture.Create<WeatherForecastEntity>();
                context.WeatherForecasts.Add(entityInDb);
                context.SaveChanges();

                var service = new WeatherForecastDataServices(context, _mapper);

                //Act
                var weatherForecast = _fixture.Create<WeatherForecast>();
                weatherForecast.Id = id;
                await service.AddOrUpdateAsync(weatherForecast);

                //Assert
                var result = context.WeatherForecasts.Find(id);

                Assert.NotNull(result);
                Assert.True(result.Summary == weatherForecast.Summary);
                Assert.True(result.Id != Guid.Empty);
                Assert.Equal(entityInDb.CreatedAt, result.CreatedAt);
                Assert.True(result.ModifiedAt > result.CreatedAt);
                Assert.True(result.ModifiedAt > DateTime.UtcNow.AddSeconds(-5));
            }
        }

        [Fact]
        public async Task DeleteAsync_Should_DeleteData()
        {
            using (var context = new OnyxDbContext(CreateNewContextOptions()))
            {
                //Arrange
                Guid id = Guid.NewGuid();
                var entityInDb = new WeatherForecastEntity
                {
                    Summary = "XAXAXA",
                };

                context.WeatherForecasts.Add(entityInDb);
                context.SaveChanges();

                var service = new WeatherForecastDataServices(context, _mapper);

                //Act
                await service.DeleteAsync(entityInDb.Id);

                //Assert
                var result = context.WeatherForecasts.Find(id);

                Assert.Null(result);
            }
        }
    }
}