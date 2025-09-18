// Domain/Post.cs
namespace Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relação com User (Autor)
        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        // Relação com Trail
        public Guid TrailId { get; set; }
        public Trail Trail { get; set; }

        // Coleções de Interações
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Contadores para Performance
        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int BookmarksCount { get; set; } = 0;
    }
}