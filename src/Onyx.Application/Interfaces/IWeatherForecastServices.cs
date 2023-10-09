using Onyx.Application.Dtos;

namespace Onyx.Application.Interfaces
{
    public interface IWeatherForecastAppServices
    {
        Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts();

        Task<Operation> CreateWeatherForecasts(WeatherForecastDto weatherForecastDto);
    }
}