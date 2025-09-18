// API/Controllers/FollowsController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class FollowsController : BaseApiController
    {
        private readonly DataContext _context;

        public FollowsController(DataContext context)
        {
            _context = context;
        }

        // POST: api/follows
        [HttpPost]
        public async Task<IActionResult> FollowUser(FollowDto followDto)
        {
            if (followDto.FollowerId == followDto.FollowingId)
            {
                return BadRequest("Você não pode seguir a si mesmo.");
            }

            var existingFollow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followDto.FollowerId && f.FollowingId == followDto.FollowingId);

            if (existingFollow != null)
            {
                return BadRequest("Você já está seguindo este usuário.");
            }

            var follow = new Follow
            {
                FollowerId = followDto.FollowerId,
                FollowingId = followDto.FollowingId
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();

            return Ok("Usuário seguido com sucesso.");
        }

        // DELETE: api/follows
        [HttpDelete]
        public async Task<IActionResult> UnfollowUser(FollowDto followDto)
        {
            var follow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == followDto.FollowerId && f.FollowingId == followDto.FollowingId);

            if (follow == null)
            {
                return NotFound("Você não está seguindo este usuário.");
            }

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();

            return Ok("Deixou de seguir o usuário com sucesso.");
        }
    }
}