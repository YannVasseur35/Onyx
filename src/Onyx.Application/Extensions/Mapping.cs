namespace Onyx.Application.Extensions
{
    public static class Mapping
    {
        public static WeatherForecastDto ToWeatherForecastDto(this WeatherForecast model)
        {
            return new WeatherForecastDto
            {
                Date = model.Date,
                TemperatureC = model.TemperatureC,
                City = model.City,
                //(...)
            };
        }

        public static WeatherForecast ToWeatherForecast(this WeatherForecastDto dto)
        {
            return new WeatherForecast
            {
                Date = dto.Date,
                TemperatureC = dto.TemperatureC,
                City = dto.City,
                //(...)
            };
        }
    }
}