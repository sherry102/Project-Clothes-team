using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    public class OrderController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        { 
            var db = new DbuniPayContext(); 
            var orders = db.Torders.ToList(); 
             
            if (orders == null || !orders.Any())
            { 
                return View(new List<OrderWrap>()); //沒有資料則返回錯誤頁面或空頁面
            }  

            List<OrderWrap> orderWrapList = new List<OrderWrap>(); 
            foreach (var order in orders)
            { 
                if (order == null) continue; 
                OrderWrap wrap = new OrderWrap()
                {
                    Oid = order.Oid,
                    Oname = order.Oname,
                    Odiscountedprice = order.Odiscountedprice,
                    OtotalPrice = order.OtotalPrice,
                    Odate = order.Odate,
                    Mid = order.Mid,
                    Oaddress = order.Oaddress,
                    Ophone = order.Ophone,
                    Ostatus = order.Ostatus,
                    Opayment = order.Opayment
                }; 
                orderWrapList.Add(wrap);
            } 
            return View(orderWrapList);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            DbuniPayContext db = new DbuniPayContext();
            Torder x = db.Torders.FirstOrDefault(c => c.Oid == id);
            if (x == null)
                return RedirectToAction("List");

            return View(new OrderWrap() { order = x });
        }
        [HttpPost]
        public IActionResult Edit(OrderWrap p)
        {
            DbuniPayContext db = new DbuniPayContext();
            Torder x = db.Torders.FirstOrDefault(c => c.Oid == p.Oid);
            if (x != null)
            { 
                x.Oname = p.Oname;
                x.Odiscountedprice = p.Odiscountedprice;
                x.OtotalPrice = p.OtotalPrice;
                x.Odate = p.Odate;
                x.Mid = p.Mid;
                x.Oaddress = p.Oaddress;
                x.Ophone = p.Ophone;
                x.Ostatus = p.Ostatus;
                x.Opayment = p.Opayment;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
