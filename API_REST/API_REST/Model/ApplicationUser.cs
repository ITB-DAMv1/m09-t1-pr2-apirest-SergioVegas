using Microsoft.AspNetCore.Identity;

namespace API_REST.Model
{
    public class ApplicationUser : IdentityUser
    {
        public List<Game> GamesU { get; set; }
    }
}
