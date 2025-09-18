// Domain/EventRegistration.cs
namespace Domain
{
    public class EventRegistration
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}