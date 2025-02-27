using DevSpot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DevSpot.Constants;
using DevSpot.Repositories;
using DevSpot.Models;

namespace DevSpot
{
    public class Program
    {
        public static async Task Main(string[] args) // Make Main asynchronous
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("database"));
            });

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();



            builder.Services.AddScoped<IRepository<JobPosting>, JobPostingRepository>();
          
            
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Perform role and user seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Seed roles and users asynchronously
                await RoleSeeder.SeedRoleAsync(services);
                await UserSeeder.SeedUserAsync(services);
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=JobPostings}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
