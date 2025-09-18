// API/Controllers/CommentsController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CommentsController : BaseApiController
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/comments/post/{postId}
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<List<CommentDto>>> GetCommentsForPost(Guid postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null) // Apenas comentários principais
                .Include(c => c.User) // Inclui o autor do comentário
                .Include(c => c.Replies) // Inclui as respostas
                    .ThenInclude(r => r.User) // Inclui o autor das respostas
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            // Mapeamento para DTO (necessário por causa das respostas aninhadas)
            var commentsDto = comments.Select(c => MapCommentToDto(c)).ToList();

            return commentsDto;
        }

        // POST: api/comments
        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentCreateDto commentDto)
        {
            var post = await _context.Posts.FindAsync(commentDto.PostId);
            if (post == null) return NotFound("Post não encontrado.");

            var comment = new Comment
            {
                Text = commentDto.Text,
                UserId = commentDto.UserId,
                PostId = commentDto.PostId,
                ParentCommentId = commentDto.ParentCommentId
            };

            post.CommentsCount++;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok("Comentário criado com sucesso.");
        }

        // Função auxiliar privada para mapear a estrutura aninhada de comentários
        private CommentDto MapCommentToDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                User = new PostAuthorDto
                {
                    Id = comment.User.Id,
                    Username = comment.User.Username,
                    Avatar = comment.User.Avatar
                },
                Replies = comment.Replies?.Select(MapCommentToDto).ToList() ?? new List<CommentDto>()
            };
        }
    }
}