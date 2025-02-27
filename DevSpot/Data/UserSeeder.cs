using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data
{
    public class UserSeeder
    {
        public static async Task SeedUserAsync(IServiceProvider serviceProvider)
        {
            var usermanager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserWithRole(usermanager, "admin@devhub.com", "Admin123!", Roles.Admin);
            await CreateUserWithRole(usermanager, "jobseeker@devhub.com", "Jobseeker123!", Roles.JobSeeker);
            await CreateUserWithRole(usermanager, "employer@devhub.com", "Employer123!", Roles.Employer);

               
        }

        private static async Task CreateUserWithRole(
      UserManager<IdentityUser> userManager,
      string email,
      string password,
      string role)
        {
            // Use the parameter `email` instead of hardcoding it
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed creating user. Error: {string.Join(",", result.Errors)}"); //Im getting error here now 
                }
            }
        }

    }
}
