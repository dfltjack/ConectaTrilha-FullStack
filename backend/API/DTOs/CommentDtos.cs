// API/DTOs/CommentDtos.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // DTO para CRIAR um comentário
    public class CommentCreateDto
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid PostId { get; set; }

        // Opcional: Se este comentário for uma resposta a outro
        public Guid? ParentCommentId { get; set; }
    }

    // DTO para RETORNAR um comentário
    // Usamos DTOs para quebrar o ciclo e formatar a resposta
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public PostAuthorDto User { get; set; } // Reutilizando DTO
        public ICollection<CommentDto> Replies { get; set; } = new List<CommentDto>();
    }
}