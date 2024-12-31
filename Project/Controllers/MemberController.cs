using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Project.Models;
using Project.ViewModel;
using System.Collections.Generic;

namespace Project.Controllers
{
    public class MemberController : Controller
    {

        public IActionResult List(CKeywordViewModel vm)
        {
            DbuniPayContext db = new DbuniPayContext();
            string keyword = vm.txtKeyword;
            IEnumerable<Tmember> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = from t in db.Tmembers
                        select t;
            else
                datas = db.Tmembers.Where
                    (p => p.Mname.Contains(keyword)
                || (keyword == "男" && p.Mgender == 0)
                || (keyword == "女" && p.Mgender == 1)
                || p.Maccount.Contains(keyword)
                || p.Memail.Contains(keyword)
                || p.Maddress.Contains(keyword)
                || p.Mbirthday.ToString().Contains(keyword)
                || p.Mphone.Contains(keyword));
            List<CMemberWrap> list = new List<CMemberWrap>();
            foreach (var t in datas)
                list.Add(new CMemberWrap() { member = t });
            return View(list);

        }

        public IActionResult Edit(int id)
        {
            DbuniPayContext db = new DbuniPayContext();
            var member = db.Tmembers.FirstOrDefault(m => m.Mid == id);
            if (member == null)
                return RedirectToAction("List");
            return View(member);
        }
        [HttpPost]
        public IActionResult Edit(Tmember p)
        {
            DbuniPayContext db = new DbuniPayContext();
            Tmember x = db.Tmembers.FirstOrDefault(c => c.Mid == p.Mid);
            if (x != null)
            {
                x.Mname = p.Mname;
                x.Mgender = p.Mgender;
                x.Maccount = p.Maccount;
                x.Mpassword = p.Mpassword;    
                x.Memail = p.Memail;
                x.Maddress = p.Maddress;
                x.Mbirthday = p.Mbirthday;
                x.Mphone = p.Mphone;
                x.Mpoints = p.Mpoints;
                x.Mpermissions = p.Mpermissions;

                db.SaveChanges();
                return RedirectToAction("List");         
            }
            return View(x);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tmember p)
        {

            DbuniPayContext db = new DbuniPayContext();
            p.McreatedDate = DateTime.Now;
            db.Tmembers.Add(p); 
            db.SaveChanges();
            return RedirectToAction("List");
                        
        }
    }
}
                 
