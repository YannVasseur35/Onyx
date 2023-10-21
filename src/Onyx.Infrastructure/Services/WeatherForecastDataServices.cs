namespace Onyx.Infrastructure.Services
{
    public class WeatherForecastDataServices : IWeatherForecastDataServices
    {
        public async Task<IEnumerable<WeatherForecast>?> GetAllAsync()
        {
            var mockList = new List<WeatherForecast>()
            {
                new WeatherForecast()
                {
                     TemperatureC = 19,
                     City = "Paris"
                },
                new WeatherForecast()
                {
                     TemperatureC = 24,
                     City = "Rennes"
                }
            };

            return mockList;
        }

        public Task<WeatherForecast?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateAsync(WeatherForecast entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}