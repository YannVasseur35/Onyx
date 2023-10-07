namespace Onyx.Application.Dtos
{
    public class PlayerJourneyDto
    {
        public string? SessionId { get; set; }

        public string? LicenceCode { get; set; }

        public string? QuestCode { get; set; }

        public string? DeviceId { get; set; }

        public string? TeamId { get; set; }

        public string? TeamName { get; set; }

        public int TeamAvatarId { get; set; }

        public GeoCoordDto? TeamLastPosition { get; set; }

        public DateTime? LastLocalSaveAt { get; set; }

        public DateTime? LastServerSaveAt { get; set; }

        public string? QuestJsonData { get; set; }

        public List<FeedbackDto>? Feedbacks { get; set; }
    }
}
