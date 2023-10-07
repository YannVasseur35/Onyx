namespace Onyx.Core.Domain.Models
{
    public class Clue
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = "";
        public int NegativePoint { get; set; }
        public bool IsRevealed { get; set; }
    }
}