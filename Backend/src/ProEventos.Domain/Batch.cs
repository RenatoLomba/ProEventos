using System;

namespace ProEventos.Domain
{
    public class Batch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Qty { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
