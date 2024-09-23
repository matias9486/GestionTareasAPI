using GestionTareas_DataAccessLayer.Data;
using GestionTareas_DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace GestionTareasAPI
{
    public class SeedUsersAndRoles
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<GestionTareasDbContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                //crear roles
                if (!context.Roles.Any())
                {
                    var adminRole = new IdentityRole<int>("ADMIN");
                    await roleManager.CreateAsync(adminRole);

                    var userRole = new IdentityRole<int>("USER");
                    await roleManager.CreateAsync(userRole);
                }

                //crear usuarios
                if (!context.Users.Any())
                {
                    var user = new Usuario
                    {
                        UserName = "admin",
                        Email = "admin@email.com",
                        Name = "admin",                        
                    };
                    var passwordSinCodificar = "adminadmin";

                    var result = await userManager.CreateAsync(user, passwordSinCodificar);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "ADMIN");
                    }
                    else
                    {
                        //Error
                        Console.WriteLine("FALLO  CREACION USUARIO");
                    }
                }
            }
        }
    }
}
