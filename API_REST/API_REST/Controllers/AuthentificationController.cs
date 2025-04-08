using API_REST.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_REST.Controllers
{
    public class AuthentificationController : ControllerBase
    {
        [ApiController]
        [Route("api/[Controller]")]
        public class AuthController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ILogger<AuthController> _logger;
            private readonly IConfiguration _configuration;
            public AuthController(UserManager<ApplicationUser> userManager, ILogger<AuthController> logger, IConfiguration configuration)
            {
                _userManager = userManager;
                _logger = logger;
                _configuration = configuration;
            }


            [HttpPost("registre")]
            public async Task<IActionResult> Register([FromBody] RegisterDTO model)
            {
                var usuari = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var resultat = await _userManager.CreateAsync(usuari, model.Password);

                if (resultat.Succeeded)
                    return Ok("Usuari registrat");

                return BadRequest(resultat.Errors);
            }
        }

    }
}
