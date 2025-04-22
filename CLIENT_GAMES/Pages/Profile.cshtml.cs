using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLIENT_GAMES.Pages
{
    public class ProfileModel : PageModel
    {
        public string? CurrentUserName { get; private set; }
        public bool IsUserLoggedIn { get; private set; } = false;

        public ProfileModel()
        {
        }

        public IActionResult OnGet()
        {
 
            CurrentUserName = HttpContext.Session.GetString("UserName");

            if (!string.IsNullOrEmpty(CurrentUserName))
            {
                IsUserLoggedIn = true;
            }
            else
            {
                IsUserLoggedIn = false;
            }
            return Page();
        }
    }
}
