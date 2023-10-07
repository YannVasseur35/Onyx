namespace Onyx.Application.Dtos
{
    public class FeedbackDto
    {
        public string? Id { get; set; }

        public string? TeamId { get; set; }

        public string? QuestCode { get; set; }

        public string? EntityId { get; set; }

        public string? EntityTypeName { get; set; }

        public string? Comments { get; set; }

        public int LikeScore { get; set; } = 0;

        public int DifficultyScore { get; set; } = 0;
    }
}
