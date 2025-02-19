using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Project.Models;
using Project.ViewModel;
using System.Collections.Generic;
using System.Text.Json;

namespace Project.Controllers
{
    public class FrontMemberController : Controller
    {
        private readonly IWebHostEnvironment _enviro;

        public FrontMemberController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        // 檢查登入狀態的API
        [HttpGet]
        public IActionResult CheckLoginStatus()
        {
            var isLoggedIn = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER) != null;
            return Json(new { isLoggedIn });
        }

        // 處理個人圖示點擊
        [HttpGet]
        public IActionResult HandleProfileClick()
        {
            var userJson = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(userJson))
            { // 未登入時返回狀態              
                return Json(new { success = false, redirectUrl = "/Account/Login" });
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

        // 會員註冊頁面
        public IActionResult fcreate()
        {
            var model = new CMemberWrap
            {
                member = new Tmember()
                {
                    Mgender = -1
                }
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult fcreate(CMemberWrap memberWrap)
        {          
            if (!ModelState.IsValid) //若模型驗證失敗，返回表單並顯示驗證錯誤訊息
                return View(memberWrap);                     
            using var db = new DbuniPayContext();                     
            memberWrap.member.McreatedDate = DateTime.Now; //設定會員的建立日期為目前時間
            try
            {
                db.Tmembers.Add(memberWrap.member);//將會員資料加入資料庫並儲存變更               
                db.SaveChanges();//成功後導向到「登入」頁面                         
                return RedirectToAction("fprofile", "FrontMember");
            }
            catch (Exception ex)
            {                     
                ModelState.AddModelError("", $"註冊失敗: {ex.Message}"); // 中文：若發生例外，將錯誤訊息加入 ModelState，並返回表單      
                return View(memberWrap);
            }
        }
        // 登出處理
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGEDIN_USER);
            return Json(new { success = true });
        }
        public IActionResult fprofile()
        {
            DbuniPayContext db = new DbuniPayContext();
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            var member = JsonSerializer.Deserialize<Tmember>(json);
            Tmember T = db.Tmembers.FirstOrDefault(c => c.Mid == member.Mid);
            CMemberWrap C = new CMemberWrap() { member = T };
            return View(C);
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

            return RedirectToAction("Profile");
        }   
    }
}
