// API/DTOs/PostUpdateDto.cs
namespace API.DTOs
{
    public class PostUpdateDto
    {
        // Geralmente, só permitimos a edição da descrição e das imagens.
        public string? Description { get; set; }
        public List<string>? Images { get; set; }
    }
}