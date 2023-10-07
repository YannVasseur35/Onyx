namespace Onyx.Core.Domain.Models
{
    //Classe qui permet la sauvegarde de tout le parcours d'un utilisateur.
    //Enregistre les réponses aux enigmes, quiz etc...
    public class PlayerJourney
    {
        //Warning ID is used localy in a browser in indexed DB.
        //Warning, entity for database also used ID (string guid)
        public int Id { get; } = 1; //TODO remove and manage form multi quest.

        public string? SessionId { get; set; }

        public string? LicenceCode { get; set; }

        public string? QuestCode { get; set; }

        public string? DeviceId { get; set; }

        public string? TeamId { get; set; }

        public string? TeamName { get; set; }

        public int TeamAvatarId { get; set; }

        public GeoCoord? TeamLastPosition { get; set; }

        public DateTime? LastLocalSaveAt { get; set; }

        public DateTime? LastServerSaveAt { get; set; }

        public Quest? Quest { get; set; }

        public List<Feedback>? Feedbacks { get; set; } = new List<Feedback>();

        public UserPrefs? UserPrefs { get; set; } = new UserPrefs();
    }
}
