using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace THH60Customer.Controllers
{
    [Authorize(Roles = "customer")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UnderConstruction()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
