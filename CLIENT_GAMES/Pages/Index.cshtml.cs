using Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CLIENT_GAMES.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public bool IsUserLoggedIn { get; private set; } = false;
        public List<GameDTO> Games { get; set; } = new List<GameDTO>();

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("ApiGames");
            var token = HttpContext.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(token))
            {
                IsUserLoggedIn = true;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("Usuari autenticat. Token afegit a la petició GET de jocs.");
            }
            else
            {
                IsUserLoggedIn = false;
                _logger.LogInformation("Usuari no autenticat. No s'afegeix token a la petició GET de jocs.");
            }

            try
            {
                var response = await client.GetAsync("api/Games");

                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error de càrrega de dades de la llista de jocs. Codi d'estat: {response?.StatusCode}");
                    Games = new List<GameDTO>();
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    try
                    {
                        Games = JsonSerializer.Deserialize<List<GameDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new List<GameDTO>();
                        Games = Games.OrderByDescending(game => game.VoteCount)
                                     .Take(10)
                                     .ToList();
                        _logger.LogInformation("Llista de jocs carregada i deserialitzada correctament.");
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Error al deserialitzar la resposta JSON de api/Games. JSON rebut: {json}", json);
                        Games = new List<GameDTO>();
                        TempData["ErrorMessage"] = "Error al processar les dades dels jocs.";
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error de connexió en obtenir la llista de jocs d'api/Games.");
                Games = new List<GameDTO>();
                TempData["ErrorMessage"] = "No s'ha pogut connectar amb el servidor per obtenir els jocs.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat a OnGet en obtenir jocs.");
                Games = new List<GameDTO>();
                TempData["ErrorMessage"] = "Ha ocorregut un error inesperat en carregar la pàgina.";
            }
        }

        public async Task<IActionResult> OnPostAddVoteAsync(int id)
        {
            var token = HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intent d'afegir vot sense estar autenticat (Joc ID: {GameId}).", id);
                TempData["ErrorMessage"] = "Necessites iniciar sessió per votar.";
                return RedirectToPage();
            }

            var client = _httpClientFactory.CreateClient("ApiGames");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiUrl = $"api/Votes/add/{id}";
                _logger.LogInformation("Enviant petició POST a {ApiUrl} per afegir vot.", apiUrl);
                var response = await client.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Vot afegit correctament per al joc ID: {id}");
                    TempData["SuccessMessage"] = "Vot afegit amb èxit!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Error en afegir vot per al joc ID {id}. Estat: {response.StatusCode}. Detalls API: {errorContent}");

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        TempData["ErrorMessage"] = $"Error: No s'ha pogut afegir el vot ({errorContent})";
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        TempData["ErrorMessage"] = "Error d'autorització. Si us plau, torna a iniciar sessió.";
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        TempData["ErrorMessage"] = "Error: El joc no s'ha trobat.";
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        TempData["ErrorMessage"] = "Ja has votat aquest joc.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Error en registrar el vot (Codi: {response.StatusCode}).";
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error de xarxa en afegir vot per a Joc ID: {GameId}", id);
                TempData["ErrorMessage"] = "Error de connexió en intentar votar.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat a OnPostAddVoteAsync per a Joc ID: {GameId}", id);
                TempData["ErrorMessage"] = "Error inesperat en processar el teu vot.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveVoteAsync(int id)
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Intent d'eliminar vot sense estar autenticat (Joc ID: {GameId}).", id);
                TempData["ErrorMessage"] = "Necessites iniciar sessió per eliminar un vot.";
                return RedirectToPage();
            }

            var client = _httpClientFactory.CreateClient("ApiGames");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var apiUrl = $"api/Votes/remove/{id}";
                _logger.LogInformation("Enviant petició DELETE a {ApiUrl} per eliminar vot.", apiUrl);
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Vot eliminat correctament per al joc ID: {id}. Resposta API: {await response.Content.ReadAsStringAsync()}");
                    TempData["SuccessMessage"] = "Vot eliminat amb èxit!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Error en ELIMINAR vot per al joc ID {id}. Estat: {response.StatusCode}. Detalls API: {errorContent}");

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        TempData["ErrorMessage"] = "Error d'autorització. Si us plau, torna a iniciar sessió.";
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        TempData["ErrorMessage"] = $"Error: Usuari o joc no trobat ({errorContent}).";
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        TempData["ErrorMessage"] = $"Error: {errorContent}";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Error ({response.StatusCode}) en eliminar el vot: {errorContent}";
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error de xarxa en eliminar vot per a Joc ID: {GameId}", id);
                TempData["ErrorMessage"] = "Error de connexió en intentar eliminar el vot.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperat a OnPostRemoveVoteAsync per a Joc ID: {GameId}", id);
                TempData["ErrorMessage"] = "Error inesperat en processar l'eliminació del vot.";
            }

            return RedirectToPage();
        }
    }
}