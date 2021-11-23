using System.Collections.Generic;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Curriculum { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<SocialNetwork> SocialNetworks { get; set; }
        public IEnumerable<SpeakerEvent> SpeakersEvents { get; set; }
    }
}
