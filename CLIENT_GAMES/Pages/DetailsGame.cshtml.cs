using CLIENT_GAMES.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CLIENT_GAMES.Pages
{
    public class DetailsGameModel : PageModel
    {
        private readonly ILogger<DetailsGameModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public GameDTO Game { get; set; } 

        public DetailsGameModel(ILogger<DetailsGameModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            /*if (id <= 0)
            {
                Console.WriteLine("------------------------>Id=0");
            }*/
            var client = _httpClientFactory.CreateClient("ApiGames");

            try
            {
                var response = await client.GetAsync($"api/Games/{id}");

                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error de carrega de dades d'aquest joc");
                    return BadRequest(response);
                }
                else
                {                    
                    var json = await response.Content.ReadAsStringAsync();
                    Game = JsonSerializer.Deserialize<GameDTO>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest(ex);
            }
        }
    }
}
