using Onyx.Application.Dtos;
using Onyx.Core.Models.OpenWeatherMap;

namespace Onyx.Application.Interfaces
{
    public interface IWeatherForecastAppServices : IAppServices<WeatherForecastDto>
    {
        //Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts(); Replaced by GetAllAsync in IAppServices

        //Task<Operation> CreateWeatherForecasts(WeatherForecastDto weatherForecastDto); Replaced by SaveAsync in IAppServices

        Task<OpenWeatherMapResponseDto>? GetWeatherForecast(string city);
    }
}