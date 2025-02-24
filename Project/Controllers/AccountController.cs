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
            // 從 Session 中獲取用戶信息
            var memberJson = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            bool isLoggedIn = !string.IsNullOrEmpty(memberJson);
            if (isLoggedIn) // 可以在這裡添加更多檢查，例如檢查 Session 是否過期
            {
                try
                {
                    // 修正：添加null忽略運算符，避免編譯警告
                    var member = JsonSerializer.Deserialize<Tmember>(memberJson!);
                    return Json(new
                    {
                        isLoggedIn = true,
                        username = member.Mname,
                        // 修正：添加重定向URL供前端使用
                        redirectUrl = "/FrontMember/fprofile"
                    });
                }
                catch
                {
                    // Session數據損壞，清除並視為未登入
                    HttpContext.Session.Remove(CDictionary.SK_LOGEDIN_USER);
                    return Json(new
                    {
                        isLoggedIn = false,
                        redirectUrl = "/FrontMember/fcreate"
                    });
                }
            }
            return Json(new // 用戶未登入
            {
                isLoggedIn = false,
                redirectUrl = "/FrontMember/fcreate"
            });
        }

        // 修改：改進登入邏輯
        [HttpPost]
        public IActionResult Login(PersoniconViewModel m) //存入快取
        {
            Tmember? member = _context.Tmembers.FirstOrDefault(
                c => c.Maccount == m.faccount && c.Mpassword == m.fpassword
            );

            if (member != null)
            {
                // 登入成功，將用戶資料存入Session
                string json = JsonSerializer.Serialize(member);
                HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER, json);
                return RedirectToAction("FrontIndex", "FrontHome");
            }
            else
            {
                // 登入失敗，設置錯誤訊息
                ViewBag.ErrorMessage = "帳號或密碼錯誤";
                return View("~/Views/FrontMember/fcreate.cshtml");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            // 檢查是否已經登入
            if (HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER) != null)
            {
                // 已登入則重定向到首頁
                return RedirectToAction("FrontIndex", "FrontHome");
            }

            // 顯示登入表單
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            // 修改重點：將原本的 return View("fcreate", "FrontMember");
            // 改為直接指定對應的 cshtml 路徑，或改為 return View("fcreate");
            // 以確保與 POST 一致並能正確顯示錯誤訊息。
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            return View("~/Views/FrontMember/fcreate.cshtml"); // 已修改
        }
    
// 新增：登出端點
[HttpPost]
        public IActionResult Logout()
        {
            try
            {
                // 清除Session中的用戶資料
                HttpContext.Session.Remove(CDictionary.SK_LOGEDIN_USER);
                return Json(new
                {
                    success = true,
                    redirectUrl = "FrontIndex/FrontHome" // 添加重定向URL
                });
            }
            catch (Exception ex)
            {
                // 記錄錯誤並返回錯誤訊息
                _logger.LogError(ex, "登出過程發生錯誤");
                return Json(new
                {
                    success = false,
                    message = "登出過程發生錯誤"
                });
            }
        }
    }
}