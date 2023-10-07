namespace Onyx.Core.Domain.Models
{
    public enum MysteryType
    {
        None = 0,
    }

    public class Mystery
    {
        public string? Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsFound { get; set; }
        public DateTime IsFoundAt { get; set; }
        public int WinPoints { get; set; }
        public string ImageUrl { get; set; } = "";
        public string Content { get; set; } = "";
        public string Answer { get; set; } = "";
    }
}