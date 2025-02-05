using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult ChatRoom()
        {
            return View();
        }

		public IActionResult test()
		{
			DbuniPayContext db = new DbuniPayContext();
            //ViewBag.chatPeople = (from message in db.ChatMessages
            //					 join member in db.Tmembers on message.MemberId equals member.Mid
            //					 group new { message,member } by message.MemberId into grouped
            //					 select new {
            //						MId = grouped.Key,
            //						MemberName = grouped.FirstOrDefault().member.Mname,
            //						MemberPhoto =  grouped.FirstOrDefault().member.Mphoto,
            //					 })
            //					 .ToList();


            //var chatMessageViewModel = new ChatMessageViewModel
            //{
            //	chatMessages = db.ChatMessages.ToList()
            //};

            //return View(chatMessageViewModel);
            return View();
        }
	}
}
