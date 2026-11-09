using Microsoft.AspNetCore.Identity;

namespace TH_H60_A03.Services
{
    public record CustomerAccountResult(string UserId, bool Created);

    public class CustomerAccountService
    {
        public const string CustomerRole = "customer";

        private readonly UserManager<IdentityUser> _userManager;

        public CustomerAccountService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CustomerAccountResult> CreateOrLinkAccountAsync(
            string email,
            string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                if (!await _userManager.IsInRoleAsync(user, CustomerRole))
                {
                    throw new InvalidOperationException(
                        "An account with this email address already exists and is not a customer account.");
                }

                return new CustomerAccountResult(user.Id, Created: false);
            }

            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(Describe(createResult));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, CustomerRole);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                throw new InvalidOperationException(Describe(roleResult));
            }

            return new CustomerAccountResult(user.Id, Created: true);
        }

        public async Task UpdateEmailAsync(string identityUserId, string email)
        {
            var user = await _userManager.FindByIdAsync(identityUserId)
                ?? throw new InvalidOperationException(
                    "The customer's sign-in account could not be found.");

            if (string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var existing = await _userManager.FindByEmailAsync(email);

            if (existing != null && existing.Id != identityUserId)
            {
                throw new InvalidOperationException(
                    "An account with this email address already exists.");
            }

            var emailResult = await _userManager.SetEmailAsync(user, email);

            if (!emailResult.Succeeded)
            {
                throw new InvalidOperationException(Describe(emailResult));
            }

            var nameResult = await _userManager.SetUserNameAsync(user, email);

            if (!nameResult.Succeeded)
            {
                throw new InvalidOperationException(Describe(nameResult));
            }
        }

        public async Task DeleteAccountAsync(string identityUserId)
        {
            var user = await _userManager.FindByIdAsync(identityUserId);

            if (user == null)
            {
                return;
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(Describe(result));
            }
        }

        public async Task ResetPasswordAsync(string identityUserId, string password)
        {
            var user = await _userManager.FindByIdAsync(identityUserId)
                ?? throw new InvalidOperationException(
                    "The customer's sign-in account could not be found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(Describe(result));
            }
        }

        private static string Describe(IdentityResult result) =>
            string.Join(" ", result.Errors.Select(error => error.Description));
    }
}
