using API_REST.Data;
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
        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Game>>> GetFilms()
        {
            return await _context.Games.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetFilm(int id)
        {
            var film = await _context.Games.FindAsync();
            if (film == null)
            {
                return NotFound();
            }
            return film;
        }
        [HttpPost]
        public async Task<ActionResult<Game>> PostFilm(FilmInsertDTO filmDTO)
        {
            var film = new Game
            {
                Name = filmDTO.Name,
                Description = filmDTO.Description,
                DirectorId = filmDTO.DirectorId
            };
            try
            {
                await _context.Games.AddAsync(film);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var film = await _context.Games.FindAsync(id);

            if (film == null)
            {
                return NotFound("Film no encontado");
            }
            _context.Games.Remove(film);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFilm(Game film, int id)
        {
            if (film.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(film).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
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
        private bool FilmExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
