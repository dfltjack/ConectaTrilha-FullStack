// API/Controllers/TrailsController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TrailsController : BaseApiController
    {
        private readonly DataContext _context;

        public TrailsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/trails
        [HttpGet]
        public async Task<ActionResult<List<Trail>>> GetTrails()
        {
            return await _context.Trails.ToListAsync();
        }

        // GET: api/trails/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Trail>> GetTrail(Guid id)
        {
            var trail = await _context.Trails.FindAsync(id);
            if (trail == null) return NotFound();
            return trail;
        }

        // POST: api/trails
        [HttpPost]
        public async Task<ActionResult<Trail>> CreateTrail(TrailCreateDto trailDto)
        {
            // Mapeamento: Converte o DTO para a entidade Trail
            var newTrail = new Trail
            {
                Name = trailDto.Name,
                LocationName = trailDto.LocationName,
                City = trailDto.City,
                State = trailDto.State,
                Difficulty = trailDto.Difficulty,
                DistanceInKm = trailDto.DistanceInKm,
                ElevationGainInM = trailDto.ElevationGainInM,
                EstimatedTimeInMin = trailDto.EstimatedTimeInMin,
                LocationGps = trailDto.LocationGps
            };

            _context.Trails.Add(newTrail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrail), new { id = newTrail.Id }, newTrail);
        }

        // PUT: api/trails/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrail(Guid id, TrailUpdateDto trailDto)
        {
            var existingTrail = await _context.Trails.FindAsync(id);
            if (existingTrail == null) return NotFound();

            // Mapeamento parcial
            existingTrail.Name = trailDto.Name ?? existingTrail.Name;
            existingTrail.LocationName = trailDto.LocationName ?? existingTrail.LocationName;
            existingTrail.City = trailDto.City ?? existingTrail.City;
            existingTrail.State = trailDto.State ?? existingTrail.State;
            existingTrail.Difficulty = trailDto.Difficulty ?? existingTrail.Difficulty;
            existingTrail.DistanceInKm = trailDto.DistanceInKm ?? existingTrail.DistanceInKm;
            existingTrail.ElevationGainInM = trailDto.ElevationGainInM ?? existingTrail.ElevationGainInM;
            existingTrail.EstimatedTimeInMin = trailDto.EstimatedTimeInMin ?? existingTrail.EstimatedTimeInMin;
            existingTrail.LocationGps = trailDto.LocationGps ?? existingTrail.LocationGps;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/trails/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrail(Guid id)
        {
            var trail = await _context.Trails.FindAsync(id);
            if (trail == null) return NotFound();

            _context.Trails.Remove(trail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}