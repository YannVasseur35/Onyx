namespace Onyx.Core.Domain.Models
{
    public class UserPrefs
    {
        public TransportType TransportType { get; set; }

        public GameModeType GameModeType { get; set; }

        public List<Badge> Badges { get; set; } = new List<Badge>();
    }
}