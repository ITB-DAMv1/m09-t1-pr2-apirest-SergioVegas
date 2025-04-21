using CLIENT_GAMES.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CLIENT_GAMES.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger _logger;

        [BindProperty]
        public LoginDTO Login { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public LoginModel(IHttpClientFactory httpClient, ILogger<LoginModel> logging)
        {
            _httpClient = httpClient;
            _logger = logging;
        }
        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var client = _httpClient.CreateClient("ApiGames");
                var response = await client.PostAsJsonAsync("api/Auth/login", Login);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(token))
                    {
                        //Guardem en sessio (cookies) el Token amb la clau "AuthToken"
                        HttpContext.Session.SetString("AuthToken", token);

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = tokenHandler.ReadJwtToken(token); // Decodificar JWT
                        var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                        if (!string.IsNullOrEmpty(usernameClaim))
                        {
                            HttpContext.Session.SetString("UserName", usernameClaim);
                        }
                        _logger.LogInformation("Login susccesfull");
                        Response.Cookies.Append("jwtToken", token, new CookieOptions { HttpOnly = false });
                        return RedirectToPage("/Index");
                    }
                }
                else
                {
                    _logger.LogInformation("Login failed");
                    ErrorMessage = "Credencials incorrectes o accés no autoritzat.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant el login");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }

            return Page();
        }
    }
}
