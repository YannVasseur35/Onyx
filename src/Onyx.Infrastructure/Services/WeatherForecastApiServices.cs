using Microsoft.Extensions.Configuration;
using Onyx.Core.Models.OpenWeatherMap;
using System.Text.Json;

namespace Onyx.Infrastructure.Services
{
    public class WeatherForecastApiServices : IWeatherForecastApiServices
    {
        private readonly OpenWeatherMapHttpClient _httpClient;
        private readonly string _apiKey = "";

        public WeatherForecastApiServices(OpenWeatherMapHttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMapApiKey"] ?? "";
        }

        public async Task<OpenWeatherMapResponseDto?> GetForecastAsync(string city)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
#pragma warning disable S3928
                throw new ArgumentNullException(paramName: "_apiKey", message: "OpenWeatherMapApiKey must not be null");
            }

            string request = $"{_httpClient.GetBaseUrl()}/?appid={_apiKey}&units=metric&q={city}";

            HttpResponseMessage httpResponse = await _httpClient.HttpClient.GetAsync(request);

            string response = await httpResponse.Content.ReadAsStringAsync();

            var res = JsonSerializer.Deserialize<OpenWeatherMapResponseDto?>(response);

            return res;
        }
    }
}