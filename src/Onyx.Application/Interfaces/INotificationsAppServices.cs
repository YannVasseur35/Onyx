namespace Onyx.Application.Interfaces
{
    public interface INotificationsAppServices
    {
        Task WeatherAlertAsync(string summary, int temperatureC, DateTime date);
    }
}