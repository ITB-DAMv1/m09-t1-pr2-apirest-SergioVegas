using API_GAMES.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class UserSeed
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserSeed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new IdentityRole("User"));

        
        if (await _userManager.FindByEmailAsync("admin@admin.com") == null)
        {
            var adminUser = new ApplicationUser { UserName = "admin", Email = "admin@admin.com" };
            var result = await _userManager.CreateAsync(adminUser, "Admin1");
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Crear usuario normal
        if (await _userManager.FindByEmailAsync("user@user.com") == null)
        {
            var normalUser = new ApplicationUser { UserName = "user", Email = "user@user.com" };
            var result = await _userManager.CreateAsync(normalUser, "User1");
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(normalUser, "User");
        }
    }
}