using System.ComponentModel.DataAnnotations;

namespace Client.Model
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;

    }
}
