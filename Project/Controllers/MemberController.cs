using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult List()
        {
            DbuniPayContext db = new DbuniPayContext();
            List<Tmember> list = db.Tmembers.ToList();
            return View(list);
        }
    }
}
