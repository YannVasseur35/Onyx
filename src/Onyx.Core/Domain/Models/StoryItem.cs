namespace Onyx.Core.Domain.Models
{
    public class StoryItem
    {
        public string? Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int Position { get; set; }
        public string ClueContent { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string ThumbUrl { get; set; } = "";
        public List<Clue> Clues { get; set; } = new List<Clue>();
        public int RevealedNegativePoint { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFinalTreasure { get; set; }
        public bool HasNoCache { get; set; }
        public string? RevealedInformation { get; set; }
    }
}