namespace Onyx.Application.Services
{
    public class WeatherForecastAppServices : IWeatherForecastAppServices
    {
        private readonly INotificationsAppServices _notificationsService;

        public WeatherForecastAppServices(INotificationsAppServices notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public async Task<IEnumerable<WeatherForecastDto>?> GetAllWeatherForecasts()
        {
            var mockList = new List<WeatherForecastDto>()
            {
                new WeatherForecastDto()
                {
                     TemperatureC = 19,
                     City = "Paris"
                },
                new WeatherForecastDto()
                {
                     TemperatureC = 24,
                     City = "Rennes"
                }
            };

            return mockList;
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