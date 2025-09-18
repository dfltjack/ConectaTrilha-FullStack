// API/Controllers/LikesController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly DataContext _context;

        public LikesController(DataContext context)
        {
            _context = context;
        }

        // POST: api/likes
        [HttpPost]
        public async Task<IActionResult> LikePost(LikeDto likeDto)
        {
            // Verificamos se o usuário já curtiu este post para não duplicar
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == likeDto.UserId && l.PostId == likeDto.PostId);

            if (existingLike != null)
            {
                return BadRequest("Você já curtiu este post.");
            }

            // Verificamos se o post que está sendo curtido realmente existe
            var post = await _context.Posts.FindAsync(likeDto.PostId);
            if (post == null)
            {
                return NotFound("Post não encontrado.");
            }

            var like = new Like
            {
                UserId = likeDto.UserId,
                PostId = likeDto.PostId
            };

            // Incrementamos o contador de likes no post para performance
            post.LikesCount++;

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok("Post curtido com sucesso.");
        }

        // DELETE: api/likes
        [HttpDelete]
        public async Task<IActionResult> UnlikePost(LikeDto likeDto)
        {
            // Procuramos pela curtida específica a ser removida
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == likeDto.UserId && l.PostId == likeDto.PostId);

            if (like == null)
            {
                return NotFound("Curtida não encontrada.");
            }

            // Verificamos se o post existe para decrementar o contador
            var post = await _context.Posts.FindAsync(likeDto.PostId);
            if (post != null && post.LikesCount > 0)
            {
                post.LikesCount--;
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok("Curtida removida com sucesso.");
        }
    }
}