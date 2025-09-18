// API/DTOs/UserUpdateDto.cs

namespace API.DTOs
{
    public class UserUpdateDto
    {
        // No DTO, todos os campos são opcionais (string?), 
        // pois o usuário pode querer atualizar apenas um deles.
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}