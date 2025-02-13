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

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(MemberViewModel m)//存入快取
        {
            // 1. 增加詳細的偵錯日誌
            Console.WriteLine($"Login attempt - Account: {model.Account}, Password: {model.Password}");

            // 1. 首先進行必要欄位驗證
            if (model == null || string.IsNullOrWhiteSpace(model.Account) || string.IsNullOrWhiteSpace(model.Password))
            {
                string json = JsonSerializer.Serialize(T);
                HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER, json);
                return RedirectToAction("FrontIndex", "FrontHome");
            }
        }

        // 修改：改進登入邏輯
        [HttpPost]
        public IActionResult Login(PersoniconViewModel model)
        {
            try
            {
                // 2. 使用更安全的查詢方式
                var matchedAccounts = _context.Tmembers
                .Where(m =>
                    m.Mphone == model.Account.Trim() ||
                    m.Maccount == model.Account.Trim() ||
                    m.Memail == model.Account.Trim())
                .ToList();

                Console.WriteLine($"Matched Accounts count: {matchedAccounts.Count}");

                // 4. 更精確的登入驗證
                var member = matchedAccounts
                    .FirstOrDefault(m => m.Mpassword == model.Password);

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
                    Console.WriteLine($"Login failed for account: {model.Account}");

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

        }


    }
}