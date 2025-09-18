// Domain/Event.cs
namespace Domain
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MeetingPoint { get; set; }
        public int MaxParticipants { get; set; }
        public int RegisteredParticipants { get; set; }

        // Relação com User (Organizador)
        public Guid OrganizerId { get; set; }
        public User Organizer { get; set; }

        // VVV A PROPRIEDADE FALTANTE ESTÁ AQUI VVV
        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    }
}