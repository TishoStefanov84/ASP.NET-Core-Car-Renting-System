namespace CarRentingSystem.Infrastructure.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using static Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<CarRentingDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<CarRentingDbContext>();

            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[] 
            {
                new Category{Name = "Mini"},
                new Category{Name = "Economy"},
                new Category{Name = "Midsize"},
                new Category{Name = "Large"},
                new Category{Name = "SUV"},
                new Category{Name = "Vans"},
                new Category{Name = "Luxury"},
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userMenager = services.GetRequiredService<UserManager<User>>();
            var roleMenager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleMenager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole
                    {
                        Name = AdministratorRoleName
                    };

                    await roleMenager.CreateAsync(role);

                    const string userEmail = "admin@crs.com";
                    const string adminPassword = "admin12";

                    var user = new User
                    {
                        Email = userEmail,
                        UserName = userEmail,
                        FullName = AdministratorRoleName
                    };

                    await userMenager.CreateAsync(user, adminPassword);

                    await userMenager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
