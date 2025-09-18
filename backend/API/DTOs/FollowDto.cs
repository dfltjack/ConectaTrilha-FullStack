// API/DTOs/FollowDto.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class FollowDto
    {
        [Required]
        public Guid FollowerId { get; set; } // Quem está seguindo

        [Required]
        public Guid FollowingId { get; set; } // Quem está sendo seguido
    }
}