// API/Controllers/RatingsController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class RatingsController : BaseApiController
    {
        private readonly DataContext _context;

        public RatingsController(DataContext context)
        {
            _context = context;
        }

        // POST: api/ratings
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateRating(RatingCreateDto ratingDto)
        {
            var trail = await _context.Trails.FindAsync(ratingDto.TrailId);
            if (trail == null) return NotFound("Trilha não encontrada.");

            // Verifica se o usuário já avaliou esta trilha
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == ratingDto.UserId && r.TrailId == ratingDto.TrailId);

            if (existingRating != null)
            {
                // Se já existe, apenas atualiza a nota
                existingRating.Value = ratingDto.Value;
            }
            else
            {
                // Se não existe, cria uma nova avaliação
                var newRating = new Rating
                {
                    Value = ratingDto.Value,
                    UserId = ratingDto.UserId,
                    TrailId = ratingDto.TrailId
                };
                _context.Ratings.Add(newRating);
            }

            await _context.SaveChangesAsync();

            // Após salvar, recalcula a média de avaliação da trilha
            await RecalculateTrailAverageRating(ratingDto.TrailId);

            return Ok("Avaliação registrada com sucesso.");
        }

        // Função auxiliar para recalcular a média
        private async Task RecalculateTrailAverageRating(Guid trailId)
        {
            var trail = await _context.Trails.FindAsync(trailId);
            if (trail == null) return;

            var ratings = await _context.Ratings.Where(r => r.TrailId == trailId).ToListAsync();

            if (ratings.Any())
            {
                trail.AverageRating = ratings.Average(r => r.Value);
                trail.TotalRatings = ratings.Count;
            }
            else
            {
                trail.AverageRating = 0;
                trail.TotalRatings = 0;
            }

            await _context.SaveChangesAsync();
        }
    }
}