using System.Collections.Generic;

namespace ProEventos.Application.Dtos
{
    public class SpeakerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Curriculum { get; set; }
        public string ImageUri { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public IEnumerable<SocialNetworkDto> SocialNetworks { get; set; }
        public IEnumerable<SpeakerDto> Speakers { get; set; }
    }
}
