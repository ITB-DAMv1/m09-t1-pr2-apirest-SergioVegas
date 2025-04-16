using API_REST.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API_REST.Controllers
{
    [ApiController]
    [Route("api/Votes")]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost("add/{gameId}")]
        public async Task<IActionResult> AddVote(int gameId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized("Usuari no autentificat.");

            // Buscar usuari i joc
            var user = await _context.Users.Include(u => u.GamesU).FirstOrDefaultAsync(u => u.Id == userId);
            var game = await _context.Games.FindAsync(gameId);

            if (user == null || game == null) return NotFound("Usuari o joc no trobat.");

            if (user.GamesU.Contains(game)) return BadRequest("L'usuari ya ha votat aquest joc.");

            user.GamesU.Add(game);
            await _context.SaveChangesAsync();

            return Ok(" Votació feta!");
        }
        [Authorize]
        [HttpDelete("remove/{gameId}")]
        public async Task<IActionResult> RemoveVote(int gameId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized("Usuari no autentificat.");

            var user = await _context.Users.Include(u => u.GamesU).FirstOrDefaultAsync(u => u.Id == userId);
            var game = await _context.Games.FindAsync(gameId);

            if (user == null || game == null) return NotFound("Usuari o joc no trobat.");

            if (!user.GamesU.Contains(game)) return BadRequest(" L'usuari no ha votat aquest joc.");

            user.GamesU.Remove(game);
            await _context.SaveChangesAsync();

            return Ok("Votació eliminada correctament.");
        }
        [HttpGet("getvotesGame/{gameId}")]
        public async Task<IActionResult> GetVotesByGame(int gameId)
        {
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null) return NotFound("Joc no trobat.");

            var usersWhoVoted = game.Users.Select(u => new { u.Id, u.UserName }).ToList();

            return Ok(usersWhoVoted);
        }
        [HttpGet("getvotesUser")]
        public async Task<IActionResult> GetVotesByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Users
                                 .Include(u => u.GamesU) 
                                 .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound("Usuari no trobat.");

            var gamesVoted = user.GamesU
                                   .Select(game => new
                                   {
                                       GameId = game.Id,
                                       GameTitle = game.Title, 
                                       GameDescription = game.Description,
                                       GameDeveloper = game.Developer
                                   })
                                   .ToList();
            if (!gamesVoted.Any())
            {
                return NotFound("Encara no has votat cap joc.");
            }
            return Ok(gamesVoted);
        }
    }
}
