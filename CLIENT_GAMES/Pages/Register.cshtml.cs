using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Client.Model;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Client.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IConfiguration _configuration;
        public RegisterModel(IHttpClientFactory httpClient, ILogger<RegisterModel>  logger, IConfiguration configuration )
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }
        [BindProperty]
        public RegisterDTO RegisterData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string apiBaseUrl = _configuration["ApiSettings:BaseUrl"];

            var httpClient = _httpClient.CreateClient();
            httpClient.BaseAddress = new Uri(apiBaseUrl);

            var jsonContent = JsonConvert.SerializeObject(RegisterData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/auth/registre", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }

            ModelState.AddModelError(string.Empty, "Error en el registre.");
            return Page();
        }
    }
}
