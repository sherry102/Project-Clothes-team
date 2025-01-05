using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Project.Controllers
{
    public class OrderController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List(CKeywordViewModel vm, string status)
        {
            var db = new DbuniPayContext(); 
            string keyword = vm.txtKeyword; 

            status = string.IsNullOrEmpty(status) ? "All" : status;
             
            IEnumerable<Torder> orders = db.Torders.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                orders = orders.Where(p => p.Oname.Contains(keyword));
            }

            if (status != "All")
            {
                if (status == "付款完成")
                {
                    orders = orders.Where(p => p.Opayment == true); // 已付款
                }
                else if (status == "尚未付款")
                {
                    orders = orders.Where(p => p.Opayment == false); // 未付款
                }
                else
                {
                    orders = orders.Where(p => p.Ostatus == status); // 其他状态
                }
            }

            List<OrderWrap> orderWrapList = new List<OrderWrap>();
            foreach (var order in orders)
            {
                if (order != null)
                {
                    orderWrapList.Add(new OrderWrap
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
                    });
                }
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
        public IActionResult Edit(OrderWrap p) {
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
