using TH_H60_A02.Models.Entities;
using TH_H60_A02.Models.ViewModels;

namespace TH_H60_A02.Services
{
    public record CustomerOperationResult(
        bool Success,
        string? Error = null,
        bool NotFound = false,
        string? Warning = null);

    public class CustomerManagementService
    {
        private readonly CustomerService _customerService;
        private readonly CustomerAccountService _customerAccountService;
        private readonly ILogger<CustomerManagementService> _logger;

        public CustomerManagementService(
            CustomerService customerService,
            CustomerAccountService customerAccountService,
            ILogger<CustomerManagementService> logger)
        {
            _customerService = customerService;
            _customerAccountService = customerAccountService;
            _logger = logger;
        }

        public async Task<List<Customer>> GetCustomersAsync(string? search)
        {
            var customers = await _customerService.GetCustomers();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                customers = customers
                    .Where(customer =>
                        customer.FirstName.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ||
                        customer.LastName.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ||
                        customer.EmailAddress.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ||
                        customer.PhoneNumber.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase) ||
                        customer.Province.Contains(
                            term,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return customers;
        }

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            try
            {
                return await _customerService.GetCustomer(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not load customer {CustomerId}.", id);

                return null;
            }
        }

        public async Task<CustomerFormViewModel?> GetCustomerFormAsync(int id)
        {
            var customer = await GetCustomerAsync(id);

            return customer == null ? null : Map(customer);
        }

        public async Task<CustomerOperationResult> CreateCustomerAsync(
            CustomerFormViewModel model)
        {
            CustomerAccountResult? account = null;

            try
            {
                account = await _customerAccountService
                    .CreateOrLinkAccountAsync(model.EmailAddress, model.Password!);

                var customer = Map(model);
                customer.IdentityUserId = account.UserId;

                await _customerService.CreateCustomer(customer);

                return new CustomerOperationResult(true);
            }
            catch (Exception ex)
            {
                if (account?.Created == true)
                {
                    try
                    {
                        await _customerAccountService.DeleteAccountAsync(account.UserId);
                    }
                    catch (Exception cleanupError)
                    {
                        _logger.LogError(
                            cleanupError,
                            "Could not remove the sign-in account for {Email} after the customer create failed.",
                            model.EmailAddress);
                    }
                }

                return new CustomerOperationResult(false, ex.Message);
            }
        }

        public async Task<CustomerOperationResult> UpdateCustomerAsync(
            CustomerFormViewModel model)
        {
            var existing = await GetCustomerAsync(model.CustomerId);

            if (existing == null)
            {
                return new CustomerOperationResult(
                    false,
                    "Customer not found.",
                    NotFound: true);
            }

            CustomerAccountResult? createdAccount = null;
            var accountUpdated = false;

            try
            {
                if (existing.IdentityUserId != null)
                {
                    if (!string.Equals(
                        existing.EmailAddress,
                        model.EmailAddress,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        await _customerAccountService.UpdateEmailAsync(
                            existing.IdentityUserId,
                            model.EmailAddress);

                        accountUpdated = true;
                    }

                    if (!string.IsNullOrWhiteSpace(model.Password))
                    {
                        await _customerAccountService.ResetPasswordAsync(
                            existing.IdentityUserId,
                            model.Password);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.Password))
                    {
                        return new CustomerOperationResult(
                            false,
                            "This customer has no sign-in account yet. Enter a password to create one.");
                    }

                    createdAccount = await _customerAccountService
                        .CreateOrLinkAccountAsync(model.EmailAddress, model.Password);
                }

                var customer = Map(model);
                customer.IdentityUserId =
                    existing.IdentityUserId ?? createdAccount!.UserId;

                await _customerService.UpdateCustomer(customer);

                return new CustomerOperationResult(true);
            }
            catch (Exception ex)
            {
                if (createdAccount?.Created == true)
                {
                    try
                    {
                        await _customerAccountService.DeleteAccountAsync(
                            createdAccount.UserId);
                    }
                    catch (Exception cleanupError)
                    {
                        _logger.LogError(
                            cleanupError,
                            "Could not remove the sign-in account for {Email} after the customer update failed.",
                            model.EmailAddress);
                    }
                }

                if (accountUpdated)
                {
                    try
                    {
                        await _customerAccountService.UpdateEmailAsync(
                            existing.IdentityUserId!,
                            existing.EmailAddress);
                    }
                    catch (Exception cleanupError)
                    {
                        _logger.LogError(
                            cleanupError,
                            "Could not restore the sign-in email for customer {CustomerId}.",
                            existing.CustomerId);
                    }
                }

                return new CustomerOperationResult(false, ex.Message);
            }
        }

        public async Task<CustomerOperationResult> DeleteCustomerAsync(int id)
        {
            var customer = await GetCustomerAsync(id);

            if (customer == null)
            {
                return new CustomerOperationResult(
                    false,
                    "Customer not found.",
                    NotFound: true);
            }

            try
            {
                await _customerService.DeleteCustomer(id);
            }
            catch (Exception ex)
            {
                return new CustomerOperationResult(false, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(customer.IdentityUserId))
            {
                try
                {
                    await _customerAccountService.DeleteAccountAsync(
                        customer.IdentityUserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Customer {CustomerId} was deleted but its sign-in account could not be removed.",
                        id);

                    return new CustomerOperationResult(
                        true,
                        Warning: "The customer was deleted, but the linked sign-in account could not be removed.");
                }
            }

            return new CustomerOperationResult(true);
        }

        private static Customer Map(CustomerFormViewModel model)
        {
            return new Customer
            {
                CustomerId = model.CustomerId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                Province = model.Province,
                IdentityUserId = model.IdentityUserId
            };
        }

        private static CustomerFormViewModel Map(Customer customer)
        {
            return new CustomerFormViewModel
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                Province = customer.Province,
                IdentityUserId = customer.IdentityUserId
            };
        }
    }
}
