namespace Onyx.Application.Services
{
    public class NotificationsAppServices : INotificationsAppServices
    {
        private readonly ILogger<NotificationsAppServices> _logger;

        public NotificationsAppServices(ILogger<NotificationsAppServices> logger)
        {
            _logger = logger;
        }

        public Task WeatherAlertAsync(string summary, int temperatureC, DateTime date)
        {
            // This class is included for demonstration only
            // In a real app it would integrate with an SMTP server or messaging service
            _logger.LogInformation("Send Weather Alert Notification");
            return Task.CompletedTask;
        }
    }
}