// API/DTOs/RatingDto.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RatingCreateDto
    {
        [Required]
        [Range(1, 5)] // Garante que a avaliação seja entre 1 e 5
        public int Value { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TrailId { get; set; }
    }
}