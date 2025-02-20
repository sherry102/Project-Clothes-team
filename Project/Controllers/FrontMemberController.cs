using Microsoft.AspNetCore.Mvc;     // 引用 ASP.NET Core MVC 的命名空間，用於建立控制器和動作方法
using Microsoft.Extensions.Hosting; // 引用主機環境相關功能的命名空間 (IWebHostEnvironment 等)
using Project.Models;               // 引用專案中 Models 資料夾的類別（例如 Tmember、DbuniPayContext）
using Project.ViewModel;            // 引用專案中 ViewModel 資料夾的類別
using System;                       // 基本系統功能，包含DateTime等
using System.Collections.Generic;   // 提供集合 (List、Dictionary) 等相關功能
using System.Text.Json;             // 提供 JSON 序列化/反序列化功能

namespace Project.Controllers
{
    public class FrontMemberController : Controller
    {
        private readonly IWebHostEnvironment _enviro;// 存取伺服器環境資訊的變數
        public FrontMemberController(IWebHostEnvironment p)// 建構函式，注入環境變數
        {
            _enviro = p;
        }       
        [HttpGet]// 處理個人圖示點擊
        public IActionResult HandleProfileClick()
        { // 從 Session 取得使用者 JSON 字串
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(userJson))
            { // 未登入時返回狀態              
                return Json(new { success = false, redirectUrl = "/FrontMember/fcreate" });
            }           
            return Json(new// 已登入時返回選單項目
            {
                success = true,
                data = new[]
                {
                new { text = "個人資料", url = "/FrontMember/fprofile" },
                new { text = "我的訂單", url = "/Member/Orders" },
                new { text = "優惠券", url = "/Member/Coupons" },
                new { text = "登出", url = "/Account/Logout" }
                }
            });
        }               
        public IActionResult fcreate()// 會員註冊頁面
        {
            var model = new CMemberWrap
            {
                member = new Tmember()
                {
                    Mgender = -1// 預設性別設定為 -1（未選擇）
                }
            };
            return View(model); // 將 model 傳遞到 fcreate.cshtml 這個 View
        }
        [HttpPost]
        public IActionResult fcreate(CMemberWrap memberWrap)
        {
            using var db = new DbuniPayContext();
            memberWrap.member.McreatedDate = DateTime.Now; //設定會員的建立日期為目前時間
            try
            {
                db.Tmembers.Add(memberWrap.member);//將會員資料加入資料庫並儲存變更
                db.SaveChanges();//成功後導向到「登入」頁面
                return RedirectToAction("fcreate", "FrontMember");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"註冊失敗: {ex.Message}"); // 中文：若發生例外，將錯誤訊息加入 ModelState，並返回表單
                return View(memberWrap);
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGEDIN_USER);// 從 Session 中移除使用者資料
            return Json(new { success = true });// 回傳登出成功狀態（用於 Ajax）
        }
        public IActionResult fprofile()
        {
            DbuniPayContext db = new DbuniPayContext();// 建議使用 using 以確保資源釋放
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER); // 取得 Session 中的使用者 JSON
            var member = JsonSerializer.Deserialize<Tmember>(json);// 反序列化為 Tmember 物件
            Tmember T = db.Tmembers.FirstOrDefault(c => c.Mid == member.Mid);// 從資料庫中取得對應的會員資料
            CMemberWrap C = new CMemberWrap() { member = T };// 將會員資料包裝到 ViewModel 中
            return View(C);// 返回個人資料頁面
        }

        [HttpPost]
        public IActionResult fprofile(CMemberWrap t)
        {
            DbuniPayContext db = new DbuniPayContext();
            Tmember T = db.Tmembers.FirstOrDefault(c => c.Mid == t.Mid);
       
            if (T != null)
            {
                T.Mname = t.Mname;
                T.Mgender = t.Mgender.GetValueOrDefault(-1);
                T.Memail = t.Memail;
                T.Maddress = t.Maddress;
                T.Mbirthday = DateOnly.Parse(t.Mbirthday);             
                T.Mphone = t.Mphone;
                if (t.photoPath != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    T.Mphoto = photoName;
                    t.photoPath.CopyTo(new FileStream(_enviro.WebRootPath + "/Images/" + photoName, FileMode.Create));
                }
                db.SaveChanges();
            }

            string json = JsonSerializer.Serialize(T); // 序列化模型数据
            HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER, json); // 更新 Session
            return RedirectToAction("fprofile"); // 重定向到 Profile 页面
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword C)
        {
            DbuniPayContext db = new DbuniPayContext();
            var json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            Tmember member = JsonSerializer.Deserialize<Tmember>(json);
            Tmember T = db.Tmembers.FirstOrDefault(c => c.Mid == member.Mid);
            if (T != null)
            {

                T.Mpassword = C.Mpassword;
                db.SaveChanges();
            }

            return RedirectToAction("fprofile");
        }   
    }
}
