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
        private readonly ILogger<AccountController> _logger;

        public AccountController(DbuniPayContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 新增：檢查登入狀態的端點
        [HttpGet]
        public IActionResult CheckLoginStatus()
        {
            try
            {
                // 從 Session 中獲取用戶信息
                var memberJson = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
                bool isLoggedIn = !string.IsNullOrEmpty(memberJson);

                return Json(new { isLoggedIn });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查登入狀態時發生錯誤");
                return Json(new { isLoggedIn = false });
            }
        }

        // 修改：改進登入邏輯
        [HttpPost]
        public IActionResult Login(PersoniconViewModel m)//存入快取
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

        // 新增：登出端點
        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove(CDictionary.SK_LOGEDIN_USER);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登出過程發生錯誤");
                return Json(new { success = false, message = "登出過程發生錯誤" });
            }
        }
    }
}