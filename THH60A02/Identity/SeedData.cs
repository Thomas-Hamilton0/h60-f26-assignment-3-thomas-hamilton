using Microsoft.AspNetCore.Identity;

namespace TH_H60_A02.Identity
{
    public static class SeedData
    {
        public static async Task SeedRoles(IServiceProvider services)
        {
            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles =
            {
                "customer",
                "clerk",
                "manager"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }

        public static async Task SeedTestAccounts(IServiceProvider services)
        {
            var userManager =
                services.GetRequiredService<UserManager<IdentityUser>>();

            await EnsureAccount(
                userManager,
                "manager@heritagereptiles.ca",
                "Manager123!",
                "manager");

            await EnsureAccount(
                userManager,
                "clerk@heritagereptiles.ca",
                "Clerk123!",
                "clerk");

            await EnsureAccount(
                userManager,
                "customer@heritagereptiles.ca",
                "Customer123!",
                "customer");

            await EnsureAccount(
                userManager,
                "thomas@heritagereptiles.ca",
                "Customer123!",
                "customer",
                "11111111-1111-1111-1111-111111111111");

            await EnsureAccount(
                userManager,
                "alice@heritagereptiles.ca",
                "Customer123!",
                "customer",
                "22222222-2222-2222-2222-222222222222");

            await EnsureAccount(
                userManager,
                "bob@heritagereptiles.ca",
                "Customer123!",
                "customer",
                "33333333-3333-3333-3333-333333333333");
        }

        private static async Task EnsureAccount(
            UserManager<IdentityUser> userManager,
            string email,
            string password,
            string role,
            string? id = null)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    Id = id ?? Guid.NewGuid().ToString(),
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not seed the {role} account: " +
                        string.Join(
                            " ",
                            result.Errors.Select(error => error.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
