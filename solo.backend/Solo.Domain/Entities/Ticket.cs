namespace Solo.Domain.Entities
{
    public class Ticket : EntityBase
    {
        public int ParkObjectId { get; set; }
        public ParkObject ParkObject { get; set; }

        public int CustomerId { get; set; }
        public User Customer { get; set; }

        public bool Closed { get; set; }
        public TicketType Type { get; set; }
    }

    public enum TicketType
    {
        Adult,
        Child,
        Privileged
    }
}
