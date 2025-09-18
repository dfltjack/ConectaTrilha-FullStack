// API/DTOs/TrailCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LocationName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Difficulty { get; set; }
        [Required]
        public double DistanceInKm { get; set; }
        [Required]
        public int ElevationGainInM { get; set; }
        [Required]
        public int EstimatedTimeInMin { get; set; }
        [Required]
        public string LocationGps { get; set; }
    }
}