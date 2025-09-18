// Domain/Rating.cs
namespace Domain
{
    public class Rating
    {
        public Guid Id { get; set; }
        public int Value { get; set; } // Valor de 1 a 5
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid TrailId { get; set; }
        public Trail Trail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}