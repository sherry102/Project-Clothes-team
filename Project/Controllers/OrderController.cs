using Microsoft.AspNetCore.Mvc;
using Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Project.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult List()
        {
            DbuniPayContext db = new DbuniPayContext();
            var result = (from s in db.Torders select s).ToList();
            return View(result);

        }
        public IActionResult OrderDetail(int id)
        {
            DbuniPayContext db = new DbuniPayContext();
            var OrderDetails = db.TorderDetails 
                               .Where (t=>t.Oid == id )
                               .ToList();
            List<COrderDetailWarp> list = new List<COrderDetailWarp>();
            foreach (var t in OrderDetails)
                list.Add(new COrderDetailWarp() { orderdetail = t });
            return View(list);
        }
    }
}
