namespace Onyx.Application.Services
{
    public class PlayerJourneyAppServices : IPlayerJourneyAppServices
    {
        private readonly ILogger<PlayerJourneyAppServices> _logger;

        public PlayerJourneyAppServices(ILogger<PlayerJourneyAppServices> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<PlayerJourneyDto>?> GetAllAsync()
        {
            try
            {
                await Task.Delay(0);

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{System.Reflection.MethodBase.GetCurrentMethod()?.Name} {ex.Message}");
                return null;
            }
        }

        public async Task<PlayerJourneyDto?> GetBySessionIdAsync(string id)
        {
            try
            {
                await Task.Delay(0);

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{System.Reflection.MethodBase.GetCurrentMethod()?.Name} {ex.Message}");
                return null;
            }
        }
    }
}