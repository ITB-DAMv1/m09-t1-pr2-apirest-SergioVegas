using API_REST.Data;
using API_REST.DTO;
using API_REST.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_REST.Controllers
{
    [Route("api/Games")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _context.Games
                                  .Include(g => g.Users) // Incluye la relación Users para contar los votos
                                  .ToListAsync();
            var gameDTOs = games.Select(game => new GameDTO
            {
                Title = game.Title,
                Description = game.Description,
                Developer = game.Developer,
                VoteCount = game.Users.Count() // Calcula el número de votos
            }).ToList();

            return Ok(gameDTOs); // Retorna la lista de GameDTOs
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound("No s'ha trobat aquest joc.");
            }
            return game;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDTO gameDTO)
        {
            var game = new Game
            {
                Title = gameDTO.Title,
                Description = gameDTO.Description,
                Developer = gameDTO.Developer
            };
            try
            {
                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound("Joc no trobat");
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutGame(Game game, int id)
        {
            if (game.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
