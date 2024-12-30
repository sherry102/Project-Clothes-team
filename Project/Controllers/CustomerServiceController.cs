using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class CustomerServiceController : Controller
    {
        public IActionResult ChatList()
        {
            return View();
        }
    }
}
