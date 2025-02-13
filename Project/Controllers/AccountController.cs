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
        public IActionResult Login(PersoniconViewModel model)
        {
            try
            {
                // 驗證用戶憑證
                var member = _context.Tmembers.FirstOrDefault(m =>
                    m.Maccount == model.faccount &&
                    m.Mpassword == model.fpassword);

                if (member != null)
                {
                    // 登入成功
                    string memberJson = JsonSerializer.Serialize(member);
                    HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER, memberJson);

                    // AJAX 請求返回 JSON
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new
                        {
                            success = true,
                            message = "登入成功",
                            redirectUrl = Url.Action("FrontIndex", "FrontHome")
                        });
                    }

                    // 一般請求進行重定向
                    return RedirectToAction("FrontIndex", "FrontHome");
                }

                // 登入失敗
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    _logger.LogWarning("登入失敗：使用者帳號或密碼錯誤");
                    return Json(new
                    {
                        success = false,
                        message = "帳號或密碼錯誤"
                    });
                }

                // 一般請求返回視圖
                ViewBag.Error = "true";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登入過程發生錯誤");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                    {
                        success = false,
                        message = "登入過程發生錯誤，請稍後再試"
                    });
                }

                ViewBag.Error = "true";
                ViewBag.ErrorMessage = "系統發生錯誤，請稍後再試";
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