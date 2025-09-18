// API/DTOs/EventDtos.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // DTO para CRIAR um evento
    public class EventCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public string MeetingPoint { get; set; }
        [Required]
        public int MaxParticipants { get; set; }
        [Required]
        public Guid OrganizerId { get; set; }
    }

    // DTO para ATUALIZAR um evento
    public class EventUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? MeetingPoint { get; set; }
        public int? MaxParticipants { get; set; }
    }

    // DTO para RETORNAR um evento (quebrando o ciclo)
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MeetingPoint { get; set; }
        public int MaxParticipants { get; set; }
        public int RegisteredParticipants { get; set; }
        public PostAuthorDto Organizer { get; set; } // Reutilizando o DTO de autor do Post
    }
}