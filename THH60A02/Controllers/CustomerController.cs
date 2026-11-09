using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TH_H60_A02.Models.Entities;
using TH_H60_A02.Models.ViewModels;
using TH_H60_A02.Services;

namespace TH_H60_A02.Controllers
{
    [Authorize(Roles = "manager,clerk")]
    public class CustomerController : Controller
    {
        private readonly CustomerManagementService _customerManagementService;

        public CustomerController(
            CustomerManagementService customerManagementService)
        {
            _customerManagementService = customerManagementService;
        }

        public async Task<IActionResult> Index(string? search)
        {
            ViewData["Search"] = search;

            try
            {
                return View(
                    await _customerManagementService.GetCustomersAsync(search));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;

                return View(new List<Customer>());
            }
        }

        public IActionResult Create()
        {
            return View(new CustomerFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(
                    nameof(model.Password),
                    "A password is required to create the customer's sign-in account.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _customerManagementService.CreateCustomerAsync(model);

            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty,
                result.Error ?? "Could not create the customer.");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _customerManagementService.GetCustomerFormAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _customerManagementService.UpdateCustomerAsync(model);

            if (result.NotFound)
            {
                return NotFound();
            }

            if (result.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty,
                result.Error ?? "Could not update the customer.");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerManagementService.GetCustomerAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerManagementService.DeleteCustomerAsync(id);

            if (result.NotFound)
            {
                return NotFound();
            }

            if (!result.Success)
            {
                var customer = await _customerManagementService.GetCustomerAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    string.Empty,
                    result.Error ?? "Could not delete the customer.");

                return View(customer);
            }

            if (!string.IsNullOrWhiteSpace(result.Warning))
            {
                TempData["Warning"] = result.Warning;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
