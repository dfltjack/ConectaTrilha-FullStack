// API/Controllers/PostsController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetPosts() 
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Trail)
                .ToListAsync();

            // Mapeamento manual para o DTO
            var postsDto = posts.Select(post => new PostDto
            {
                Id = post.Id,
                Description = post.Description,
                Images = post.Images,
                CreatedAt = post.CreatedAt,
                Author = new PostAuthorDto
                {
                    Id = post.Author.Id,
                    Username = post.Author.Username,
                    Avatar = post.Author.Avatar
                },
                Trail = new PostTrailDto
                {
                    Id = post.Trail.Id,
                    Name = post.Trail.Name,
                    City = post.Trail.City,
                    State = post.Trail.State
                }
            }).ToList();

            return postsDto;
        }

        // GET: api/posts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(Guid id) 
        {
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Trail)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            // Mapeamento manual para o DTO
            var postDto = new PostDto
            {
                Id = post.Id,
                Description = post.Description,
                Images = post.Images,
                CreatedAt = post.CreatedAt,
                Author = new PostAuthorDto
                {
                    Id = post.Author.Id,
                    Username = post.Author.Username,
                    Avatar = post.Author.Avatar
                },
                Trail = new PostTrailDto
                {
                    Id = post.Trail.Id,
                    Name = post.Trail.Name,
                    City = post.Trail.City,
                    State = post.Trail.State
                }
            };

            return postDto;
        }

        // POST: api/posts    
        [HttpPost]
        public async Task<ActionResult<PostDto>> CreatePost(PostCreateDto postDto)
        {
            var author = await _context.Users.FindAsync(postDto.AuthorId);
            var trail = await _context.Trails.FindAsync(postDto.TrailId);

            if (author == null) return BadRequest("Autor inválido.");
            if (trail == null) return BadRequest("Trilha inválida.");

            var newPost = new Post
            {
                Description = postDto.Description,
                Images = postDto.Images,
                AuthorId = postDto.AuthorId,
                TrailId = postDto.TrailId,
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            var postDtoToReturn = new PostDto
            {
                Id = newPost.Id,
                Description = newPost.Description,
                Images = newPost.Images,
                CreatedAt = newPost.CreatedAt,
                Author = new PostAuthorDto
                {
                    Id = author.Id,
                    Username = author.Username,
                    Avatar = author.Avatar
                },
                Trail = new PostTrailDto
                {
                    Id = trail.Id,
                    Name = trail.Name,
                    City = trail.City,
                    State = trail.State
                }
            };

            return CreatedAtAction(nameof(GetPost), new { id = postDtoToReturn.Id }, postDtoToReturn);
        }

        // PUT: api/posts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, PostUpdateDto postDto)
        {
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null) return NotFound();

            existingPost.Description = postDto.Description ?? existingPost.Description;
            existingPost.Images = postDto.Images ?? existingPost.Images;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/posts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}