namespace Onyx.Core.Domain.Models
{
    public class BluffQuiz
    {
        public string? Name { get; set; }
        public string? Answer { get; set; }
        public bool IsDone { get; set; }
        public DateTime IsDoneAt { get; set; }
        public List<string> Clues { get; set; } = new List<string>();
        public List<string> UserAnswers { get; set; } = new List<string>();
        public DateTime StartDate { get; set; }
        public int TimeBetweenNewClueInMinute { get; set; }
        public int MaxPoints { get; set; }
        public int UserPoints { get; set; }
        public int DecotePerAnswer { get; set; }
    }
}