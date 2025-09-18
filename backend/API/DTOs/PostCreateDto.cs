// API/DTOs/PostCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class PostCreateDto
    {
        public string? Description { get; set; }

        [Required]
        public List<string> Images { get; set; } = new List<string>();

        // Chaves estrangeiras que serão enviadas pelo App
        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public Guid TrailId { get; set; }
    }
}