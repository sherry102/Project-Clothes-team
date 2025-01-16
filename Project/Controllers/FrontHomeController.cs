using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class FrontHomeController : Controller
    {
        public IActionResult Customize()
        {
            return View();
        }
        public IActionResult ChangeClothes()
        {
            return View();
        }
    }
}
