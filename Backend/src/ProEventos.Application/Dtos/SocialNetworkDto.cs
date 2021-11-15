namespace ProEventos.Application.Dtos
{
    public class SocialNetworkDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? EventId { get; set; }
        public EventDto Event { get; set; }
        public int? SpeakerId { get; set; }
        public SpeakerDto Speaker { get; set; }
    }
}
