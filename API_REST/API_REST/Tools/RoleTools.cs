using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;

namespace API_REST.Tools
{
    public static class RoleTools
    {
        public static async Task CrearRolsInicials(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] rols = { "Admin" };

            foreach (var rol in rols)
            {
                
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }
        }
    } 

}
