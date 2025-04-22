using System.ComponentModel.DataAnnotations;

namespace CLIENT_GAMES.Model
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string? Name { get; set; }
    }
}
