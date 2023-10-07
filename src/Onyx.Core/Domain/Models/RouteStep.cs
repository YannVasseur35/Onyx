namespace Onyx.Core.Domain.Models
{
    public class RouteStep
    {
        public int Position { get; set; }
        public int AdverageTime { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string CityTown { get; set; } = "";
        public string CompleteAddress { get; set; } = "";
        public GeoCoord? RdvPoint { get; set; }
        public List<string>? ImageUrls { get; set; }
        public bool IsDone { get; set; }
        public bool IsSecret { get; set; }
        public Instruction? Instructions { get; set; }
        public List<Mystery> Mysteries { get; set; } = new List<Mystery>();
        public List<Quiz> MiniQuizs { get; set; } = new List<Quiz>();
        public List<GeoCache> GeoCaches { get; set; } = new List<GeoCache>();
        public List<GeoCache> QuestCaches { get; set; } = new List<GeoCache>();
        public List<InfoCulture> InfoCultures { get; set; } = new List<InfoCulture>();
    }
}