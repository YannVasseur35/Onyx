namespace Onyx.Application.Interfaces
{
    public interface IPlayerJourneyAppServices
    {
        public Task<IEnumerable<PlayerJourneyDto>?> GetAllAsync();

        public Task<PlayerJourneyDto?> GetBySessionIdAsync(string id);
    }
}