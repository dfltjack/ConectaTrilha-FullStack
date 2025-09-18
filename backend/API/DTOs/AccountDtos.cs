// API/DTOs/AccountDtos.cs
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,16}$", ErrorMessage = "A senha deve ser complexa")]
        public string Password { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    // DTO que retornaremos para o cliente após o login/registro
    public class UserTokenDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}