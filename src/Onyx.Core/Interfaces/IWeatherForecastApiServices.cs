using Onyx.Core.Models.OpenWeatherMap;

namespace Onyx.Core.Interfaces
{
    public interface IWeatherForecastApiServices
    {
        Task<OpenWeatherMapResponseDto?> GetForecastAsync(string city);
    }
}