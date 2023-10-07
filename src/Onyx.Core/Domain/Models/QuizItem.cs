namespace Onyx.Core.Domain.Models
{
    public class QuizItem
    {
        public string Id { get; } = Guid.NewGuid().ToString();

        public string Question { get; set; } = "";
        public string MediaSource { get; set; } = "";
        public bool AudioLoop { get; set; } = false;
        public int TimeToAnswerInSecond { get; set; }
        public List<string> Choices { get; set; }
        public int AnswerIndex { get; set; }
        public int UserAnswerIndex { get; set; } = -1;
        public int WinPoints { get; set; }

        public QuizItem()
        {
            Choices = new List<string>();
        }
    }
}