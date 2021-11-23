using System;
using System.Collections.Generic;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public DateTime? EventDate { get; set; }
        public string Theme { get; set; }
        public int PeopleQty { get; set; }
        public string ImageUri { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Batch> Batches { get; set; }
        public IEnumerable<SocialNetwork> SocialNetworks { get; set; }
        public IEnumerable<SpeakerEvent> SpeakersEvents { get; set; }
    }
}
