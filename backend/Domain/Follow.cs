// Domain/Follow.cs
namespace Domain
{
    public class Follow
    {
        // Chave primária composta será configurada no DataContext
        public Guid FollowerId { get; set; }
        public User Follower { get; set; }
        public Guid FollowingId { get; set; }
        public User Following { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}