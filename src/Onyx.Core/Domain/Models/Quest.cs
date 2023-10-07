namespace Onyx.Core.Domain.Models
{
    public class Quest
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsQuestStarted => StartDate < DateTime.Now;
        public int DurationHours { get; set; }
        public QuestType Type { get; set; }


        //Activité :

        public List<RouteStep> RouteSteps { get; set; } = new List<RouteStep>();

        public List<StoryItem> StoryItems { get; set; } = new List<StoryItem>();

        public BluffQuiz? BluffQuiz { get; set; }

        public List<Quiz> ChallengeQuizs { get; set; } = new List<Quiz>();
    }
}