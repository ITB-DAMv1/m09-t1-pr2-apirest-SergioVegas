using Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public List<GameDTO> Games { get; set; } = new List<GameDTO>();

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("ApiGames");

            try
            {
                
                var response = await client.GetAsync("api/Games");
                //var response = await client.GetFromJsonAsAsyncEnumerable<List<Film>>("Films",);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error de carrega de dades de la llista de jocs");
                }
                else
                {
                    
                    var json = await response.Content.ReadAsStringAsync();
                    Games = JsonSerializer.Deserialize<List<GameDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    Games = Games.OrderByDescending(game => game.VoteCount).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            
            var token = HttpContext.Session.GetString("AuthToken");
            if (!Tools.TokenHelper.IsTokenSession(token)) return RedirectToPage("/Login");

            //Obliguem al HttpClient a enviar el token en el Header:
            var client = _httpClientFactory.CreateClient("ApiGames");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/Films/delete/{id}");

            //En aquest cas comprobem si la resposta es BadRequest
            if (response == null || !(response.StatusCode == HttpStatusCode.BadRequest))
            {
                _logger.LogError("No s'ha eliminat l'element. BadRequest response");
            }

            return RedirectToPage();
        }
    }
}
