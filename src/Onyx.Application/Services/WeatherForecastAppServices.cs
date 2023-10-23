using Onyx.Core.Interfaces;
using Onyx.Application.Extensions;

namespace Onyx.Application.Services
{
    public class WeatherForecastAppServices : IWeatherForecastAppServices
    {
        private readonly INotificationsAppServices _notificationsService;
        private readonly IWeatherForecastDataServices _weatherForecastDataServices;

        public WeatherForecastAppServices(
            INotificationsAppServices notificationsService,
            IWeatherForecastDataServices weatherForecastDataServices)
        {
            _notificationsService = notificationsService;
            _weatherForecastDataServices = weatherForecastDataServices;
        }

        public async Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts()
        {
            var weatherForecastList = await _weatherForecastDataServices.GetAllAsync();

            return weatherForecastList.Select(x => x.ToWeatherForecastDto());
        }

        public async Task<Operation> CreateWeatherForecasts(WeatherForecastDto weatherForecastDto)
        {
            //Todo: creation of weatherForecastDto

            if (weatherForecastDto.TemperatureC <= 0)
            {
                await _notificationsService.WeatherAlertAsync("Attention risque de gel", 0, DateTime.UtcNow);
            }

            return new Operation();
        }
    }
}