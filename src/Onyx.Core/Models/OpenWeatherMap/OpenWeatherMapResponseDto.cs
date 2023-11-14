#pragma warning disable IDE1006

namespace Onyx.Core.Models.OpenWeatherMap
{
    public class OpenWeatherMapResponseDto
    {
        public float lat { get; set; }
        public float lon { get; set; }
        public string? timezone { get; set; }
        public int timezone_offset { get; set; }
        public Current? current { get; set; }
        public object[]? minutely { get; set; }
        public object[]? hourly { get; set; }
        public object[]? daily { get; set; }
        public Alert[]? alerts { get; set; }
    }

    public class Current
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public float temp { get; set; }
        public float feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public float dew_point { get; set; }
        public float uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public float wind_speed { get; set; }
        public int wind_deg { get; set; }
        public float wind_gust { get; set; }
        public Weather[]? weather { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string? main { get; set; }
        public string? description { get; set; }
        public string? icon { get; set; }
    }

    public class Alert
    {
        public string? sender_name { get; set; }
        public string? _event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string? description { get; set; }
        public object[]? tags { get; set; }
    }
}