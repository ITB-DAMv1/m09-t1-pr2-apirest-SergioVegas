using Microsoft.AspNetCore.Identity;

namespace API_GAMES.Model
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Game> GamesU { get; set; }
    }
}
