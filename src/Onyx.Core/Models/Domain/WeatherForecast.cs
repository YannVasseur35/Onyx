namespace Onyx.Core.Models.Domain
{
    public class WeatherForecast
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? Summary { get; set; }
        public bool Current { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public Coordinates Coordinates { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class Coordinates
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}