namespace Onyx.Application.Dtos
{
    public class WeatherForecastDto
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? Summary { get; set; }
        public bool Current { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}