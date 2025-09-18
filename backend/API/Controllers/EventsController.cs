// API/Controllers/EventsController.cs
using API.DTOs;
using Domain;
using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class EventsController : BaseApiController
    {
        private readonly DataContext _context;

        public EventsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Organizer)
                .ToListAsync();

            // Mapeamento para DTO
            var eventsDto = events.Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                MeetingPoint = e.MeetingPoint,
                MaxParticipants = e.MaxParticipants,
                RegisteredParticipants = e.RegisteredParticipants,
                Organizer = new PostAuthorDto
                {
                    Id = e.Organizer.Id,
                    Username = e.Organizer.Username,
                    Avatar = e.Organizer.Avatar
                }
            }).ToList();

            return eventsDto;
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(Guid id)
        {
            var anEvent = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (anEvent == null) return NotFound();

            // Mapeamento para DTO
            var eventDto = new EventDto
            {
                Id = anEvent.Id,
                Title = anEvent.Title,
                Description = anEvent.Description,
                StartTime = anEvent.StartTime,
                EndTime = anEvent.EndTime,
                MeetingPoint = anEvent.MeetingPoint,
                MaxParticipants = anEvent.MaxParticipants,
                RegisteredParticipants = anEvent.RegisteredParticipants,
                Organizer = new PostAuthorDto
                {
                    Id = anEvent.Organizer.Id,
                    Username = anEvent.Organizer.Username,
                    Avatar = anEvent.Organizer.Avatar
                }
            };

            return eventDto;
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(EventCreateDto eventDto)
        {
            var organizer = await _context.Users.FindAsync(eventDto.OrganizerId);
            if (organizer == null) return BadRequest("Organizador inválido.");

            var newEvent = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                StartTime = eventDto.StartTime,
                EndTime = eventDto.EndTime,
                MeetingPoint = eventDto.MeetingPoint,
                MaxParticipants = eventDto.MaxParticipants,
                OrganizerId = eventDto.OrganizerId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            // Note: O retorno aqui não será o DTO para simplificar, mas em um projeto real, seria.
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
        }

        // POST: api/events/{id}/register
        [HttpPost("{id}/register")]
        public async Task<IActionResult> RegisterForEvent(Guid id, [FromBody] Guid userId)
        {
            var anEvent = await _context.Events.FindAsync(id);
            if (anEvent == null) return NotFound("Evento não encontrado.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("Usuário não encontrado.");

            if (anEvent.RegisteredParticipants >= anEvent.MaxParticipants)
            {
                return BadRequest("Não há mais vagas para este evento.");
            }

            var existingRegistration = await _context.Set<EventRegistration>()
                .AnyAsync(r => r.EventId == id && r.UserId == userId);

            if (existingRegistration)
            {
                return BadRequest("Usuário já está inscrito neste evento.");
            }

            var registration = new EventRegistration
            {
                EventId = id,
                UserId = userId
            };

            anEvent.RegisteredParticipants++; // Incrementa o contador

            _context.Set<EventRegistration>().Add(registration);
            await _context.SaveChangesAsync();

            return Ok("Inscrição realizada com sucesso.");
        }
    }
}