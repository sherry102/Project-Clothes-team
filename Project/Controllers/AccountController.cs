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
            // 1. 增加詳細的偵錯日誌
            Console.WriteLine($"Login attempt - Account: {model.Account}, Password: {model.Password}");

            // 1. 首先進行必要欄位驗證
            if (model == null || string.IsNullOrWhiteSpace(model.Account) || string.IsNullOrWhiteSpace(model.Password))
            {
                string json = JsonSerializer.Serialize(T);
                HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER, json);
                return RedirectToAction("FrontIndex", "FrontHome");
            }

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
             
                    // 5. 使用更安全的序列化方式
                    var memberInfo = new Dictionary<string, object>
                    {
                        { "Mid", member.Mid },
                        { "Mname", member.Mname },
                        { "Maccount", member.Maccount }
                    };
                                
                    HttpContext.Session.SetString(
                        CDictionary.SK_LOGEDIN_USER,
                        System.Text.Json.JsonSerializer.Serialize(memberInfo)
                    );
                    Console.WriteLine($"User {member.Maccount} logged in successfully");
                 

                    return Json(new
                    {
                        success = true,
                        message = "登入成功！",
                        redirectUrl = "/FrontHome/FrontIndex",
                         memberInfo = new  // 新增會員信息
                         {
                             Mname = member.Mname,
                             Maccount = member.Maccount
                         }
                    });
                }
                else
                {
                    Console.WriteLine($"Login failed for account: {model.Account}");

                    return Json(new
                    {
                        success = false,
                        message = "帳號或密碼錯誤",
                        debugInfo = new
                        {
                            matchedAccountsCount = matchedAccounts.Count
                        }
                    });
                }            
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Login error: {ex.Message}");

                return Json(new
                {
                    success = false,
                    message = "系統發生錯誤，請稍後再試"
                });
            }
        }
        // 登出方法
        [HttpPost]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return Json(new
            {
                success = true,
                message = "登出成功"
            });
        }
        // 在這裡加入新的 CheckLoginStatus 方法
        [HttpGet]
        public JsonResult CheckLoginStatus()
        {
            try
            {
                var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);

        }


    }
}
