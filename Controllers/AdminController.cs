using Microsoft.AspNetCore.Mvc;

namespace SuperMarketSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
