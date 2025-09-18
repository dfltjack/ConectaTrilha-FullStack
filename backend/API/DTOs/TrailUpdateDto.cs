// API/DTOs/TrailUpdateDto.cs
namespace API.DTOs
{
    public class TrailUpdateDto
    {
        public string? Name { get; set; }
        public string? LocationName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Difficulty { get; set; }
        public double? DistanceInKm { get; set; }
        public int? ElevationGainInM { get; set; }
        public int? EstimatedTimeInMin { get; set; }
        public string? LocationGps { get; set; }
    }
}