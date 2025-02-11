using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.ViewModel;
using System.Diagnostics;
using System.Text.Json;
using System.Linq;


namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbuniPayContext _context;

        public AccountController(DbuniPayContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(MemberViewModel m)//存入快取
        {
            DbuniPayContext db = new DbuniPayContext();
            Tmember T = db.Tmembers.FirstOrDefault(c => c.Maccount == m.faccount && c.Mpassword == m.fpassword);
            string Error = "false";
            if (T != null && T.Mpassword == m.fpassword)
            {
                string json = JsonSerializer.Serialize(T);
                HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER, json);
                return RedirectToAction("FrontIndex", "FrontHome");
            }
            else
            {
                Error = "true";
                ViewBag.Error = Error;
                return View();
            }

        }


    }
}
