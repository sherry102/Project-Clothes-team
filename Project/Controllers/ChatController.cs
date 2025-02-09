using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class ChatController : Controller
    {
        private readonly DbuniPayContext _dbuniPayContext;
        public IActionResult ChatRoom()
        {
            return View();
        }

		//public IActionResult test()
		//{
		//	DbuniPayContext db = new DbuniPayContext();
            
  //          return View();
  //      }

        // 取得會員資訊
        [HttpGet]
        public IActionResult GetMemberInfo()
        {
            if (!Int32.TryParse(HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER), out int memberId))
            {
                return Json(new { success = false, message = "用戶未登入。" });
            }
            var member = _dbuniPayContext.Tmembers.FirstOrDefault(p => p.Mid == memberId);
            if (member == null)
            {
                return Json(new { success = false, message = "會員資料不存在。" });
            }
            return Json(new
            {
                success = true,
                data = new
                {
                    id = member.Mid,
                    name = member.Mname,
                    gender = member.Mgender,
                    account = member.Maccount,
                    email = member.Memail,
                    address = member.Maddress,
                    birthday = member.Mbirthday.ToString("yyyy-MM-dd"),
                    phone = member.Mphone,
                    points = member.Mpoints,
                    permissions = member.Mpermissions,
                    createdDate = member.McreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    photo = member.Mphoto,
                    isHided = member.MisHided
                }
            });
        }
    }
}
