// API/Controllers/BookmarksController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BookmarksController : BaseApiController
    {
        private readonly DataContext _context;

        public BookmarksController(DataContext context)
        {
            _context = context;
        }

        // POST: api/bookmarks
        [HttpPost]
        public async Task<IActionResult> BookmarkPost(LikeDto bookmarkDto) // Reutilizando o LikeDto
        {
            var existingBookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.UserId == bookmarkDto.UserId && b.PostId == bookmarkDto.PostId);

            if (existingBookmark != null)
            {
                return BadRequest("Você já salvou este post.");
            }

            var post = await _context.Posts.FindAsync(bookmarkDto.PostId);
            if (post == null)
            {
                return NotFound("Post não encontrado.");
            }

            var bookmark = new Bookmark
            {
                UserId = bookmarkDto.UserId,
                PostId = bookmarkDto.PostId
            };

            post.BookmarksCount++;

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();

            return Ok("Post salvo com sucesso.");
        }

        // DELETE: api/bookmarks
        [HttpDelete]
        public async Task<IActionResult> UnbookmarkPost(LikeDto bookmarkDto) // Reutilizando o LikeDto
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.UserId == bookmarkDto.UserId && b.PostId == bookmarkDto.PostId);

            if (bookmark == null)
            {
                return NotFound("Post salvo não encontrado.");
            }

            var post = await _context.Posts.FindAsync(bookmarkDto.PostId);
            if (post != null && post.BookmarksCount > 0)
            {
                post.BookmarksCount--;
            }

            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();

            return Ok("Post salvo removido com sucesso.");
        }
    }
}