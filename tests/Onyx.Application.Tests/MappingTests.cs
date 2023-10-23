using AutoMapper;
using Onyx.Application.Dtos;
using Onyx.Application.Mappings;
using Onyx.Core.Models.Domain;

namespace Onyx.Application.Tests
{
    public class MappingTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;

        public MappingTests()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });

            _mapper = _config.CreateMapper();
        }

        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            //arrange

            //act

            //assert
            _config.AssertConfigurationIsValid();
        }

        [Fact]
        public void AutoMapper_Map_WeatherForecast_To_WeatherForecastDto()
        {
            //arrange
            var model = new WeatherForecast()
            {
                City = "Rennes",
                TemperatureC = 1,
                Summary = "fait pas chaud",
            };

            //act
            var dto = _mapper.Map<WeatherForecastDto>(model);

            //assert
            Assert.NotNull(dto);
            Assert.Equal(model.City, dto.City);
            Assert.Equal(model.TemperatureC, dto.TemperatureC);
            Assert.Equal(model.Summary, dto.Summary);
        }

        [Fact]
        public void AutoMapper_Map_WeatherForecastDto_To_WeatherForecast()
        {
            //arrange
            var dtoMock = new WeatherForecastDto()
            {
                City = "Rennes",
                TemperatureC = 1,
                Summary = "fait pas chaud",
            };

            //act
            var model = _mapper.Map<WeatherForecast>(dtoMock);

            //assert
            Assert.NotNull(model);
            Assert.Equal(dtoMock.City, model.City);
            Assert.Equal(dtoMock.TemperatureC, model.TemperatureC);
            Assert.Equal(dtoMock.Summary, model.Summary);
        }

        [Fact]
        public void AutoMapper_Map_Coords_WeatherForecast_To_WeatherForecastDto()
        {
            //arrange
            var model = new WeatherForecast()
            {
                Coordinates = new()
                {
                    Latitude = 44,
                    Longitude = 55
                }
            };

            //act
            var dto = _mapper.Map<WeatherForecastDto>(model);

            //assert
            Assert.NotNull(dto);
            Assert.Equal(model.Coordinates.Latitude, dto.Latitude);
            Assert.Equal(model.Coordinates.Longitude, dto.Longitude);
        }

        [Fact]
        public void AutoMapper_Map_Coords_WeatherForecastDto_To_WeatherForecast()
        {
            //arrange
            var dtoMock = new WeatherForecastDto()
            {
                Latitude = 12,
                Longitude = 13
            };

            //act
            var model = _mapper.Map<WeatherForecast>(dtoMock);

            //assert
            Assert.NotNull(model);
            Assert.Equal(dtoMock.Latitude, model.Coordinates.Latitude);
            Assert.Equal(dtoMock.Longitude, model.Coordinates.Longitude);
        }
    }
}