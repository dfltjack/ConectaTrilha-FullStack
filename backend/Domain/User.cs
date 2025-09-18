// Domain/User.cs
namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // Relações
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();

        // VVV ADICIONE AS COLEÇÕES FALTANTES AQUI VVV
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}