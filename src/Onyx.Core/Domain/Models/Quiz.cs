namespace Onyx.Core.Domain.Models
{
    public class Quiz
    {
        public string? Id { get; set; }
        public string Name { get; set; } = "";
        public int MaxQuestions { get; set; }
        public bool IsDone { get; set; }
        public DateTime IsDoneAt { get; set; }
        public bool IsShuffled { get; set; }
        public bool ShowRightAnswer { get; set; }
        public QuizType QuizType { get; set; }
        public List<QuizItem> QuizItems { get; set; }

        public Quiz()
        {
            QuizItems = new List<QuizItem>();
        }
    }
}