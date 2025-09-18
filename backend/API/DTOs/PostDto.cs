// API/DTOs/PostDto.cs

namespace API.DTOs
{
    // Um DTO simplificado para o User, apenas com o essencial
    public class PostAuthorDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? Avatar { get; set; }
    }

    // Um DTO simplificado para a Trail
    public class PostTrailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    // O DTO principal para o Post
    public class PostDto
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }

        public PostAuthorDto Author { get; set; }
        public PostTrailDto Trail { get; set; }
    }
}