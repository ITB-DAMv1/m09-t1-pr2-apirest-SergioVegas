using API_GAMES.Data;
using API_GAMES.DTO;
using API_GAMES.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_GAMES.Controllers
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
                                  .Include(g => g.Users) 
                                  .ToListAsync();
            var gameDTOs = games.Select(game => new GameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Developer = game.Developer,
                Image = game.Image,
                VoteCount = game.Users.Count() // Calcula el nomrbe de vots
            }).ToList();

            return Ok(gameDTOs); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound("No s'ha trobat aquest joc.");
            }
            var gameDto = new GameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Developer = game.Developer,
                Description = game.Description,
                Image = game.Image,
                VoteCount = game.Users.Count(),
            };
            return Ok(gameDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDTO gameDTO)
        {
            var game = new Game
            {
                Title = gameDTO.Title,
                Description = gameDTO.Description,
                Developer = gameDTO.Developer,
                Image = gameDTO.Image,
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
