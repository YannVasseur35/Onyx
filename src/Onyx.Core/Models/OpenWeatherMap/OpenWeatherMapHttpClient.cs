namespace Onyx.Core.Models.OpenWeatherMap
{
    public class OpenWeatherMapHttpClient
    {
        private readonly HttpClient _httpClient;
        public HttpClient HttpClient => _httpClient;

        public OpenWeatherMapHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            string APIOpenWeatherMapBaseUrl = configuration["OpenWeatherMapApiBaseUrl"] ?? "";

            _httpClient.BaseAddress = new Uri(APIOpenWeatherMapBaseUrl);
        }

        public string? GetBaseUrl()
        {
            return _httpClient.BaseAddress?.ToString();
        }
    }
}