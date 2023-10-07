namespace Onyx.Core.Domain.Models
{
    public class GeoCache
    {
        public string? Id { get; set; }
        public string? StoryItemId { get; set; }
        public string? HumanCacheCode { get; set; }
        public string? OldCacheCode { get; set; }
        public string? CacheCode { get; set; }         
        public string? Name { get; set; }
        public string? QuestName { get; set; }
        public string? QuestCode { get; set; }
        public string? Warning { get; set; }
        public string? PublicRedirectionTarget { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double CoordsAccuracy { get; set; }
        public bool IsFinalTreasure { get; set; }
        public bool IsSecret { get; set; }
        public bool IsFound { get; set; }         
        public DateTime? IsFoundAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? IsActiveModifyAt { get; set; }
        public int Difficulty { get; set; }
        public string? ClueInfo { get; set; }
        public int RadiusInMeter { get; set; }
        public string CircleColor { get; set; } = "#A33E00";
        public double CircleOpacity { get; set; } = 0.6;
        public string? FoundDescription { get; set; }
        public string? FoundImageUrl { get; set; }
        public int WinPoints { get; set; }
    }
}