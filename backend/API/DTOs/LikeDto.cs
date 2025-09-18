// API/DTOs/LikeDto.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LikeDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid PostId { get; set; }
    }
}