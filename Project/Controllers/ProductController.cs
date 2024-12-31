﻿using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.ViewModel;

namespace Project.Controllers
{
    public class ProductController : Controller
    {
        IWebHostEnvironment _enviro = null;
        public ProductController(IWebHostEnvironment p)
        {
            _enviro = p;
        }

        public IActionResult List(ProductViewModel vm, int id)
        {
            DbuniPayContext db = new DbuniPayContext();
            string keyword = vm.txtKeyword;
            IEnumerable<Tproduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = db.Tproducts
                        .Where(t => !t.IsHided);  // 過濾已刪除的記錄
            else
                datas = db.Tproducts.Where(t => t.Pdepiction.Contains(keyword));
            List<CProductWrap> list = new List<CProductWrap>();
            foreach (var t in datas)
                list.Add(new CProductWrap() { product = t });
            return View(list);
        }

        //下架
        public IActionResult Hide(int? id)
        {
            if (id != null)
            {
                DbuniPayContext db = new DbuniPayContext();
                Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == id);
                if (x != null)
                {
                    x.IsHided = true;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CProductWrap p)
        {
            if (p.photoPath != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                p.Pimage = photoName;
                p.photoPath.CopyTo(new FileStream(_enviro.WebRootPath + "/images/" + photoName,FileMode.Create));
            }
            DbuniPayContext db = new DbuniPayContext();
            p.Pdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            db.Tproducts.Add(p.product);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            DbuniPayContext db = new DbuniPayContext();
            Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == id);
            if (x == null)
                return RedirectToAction("List");

            return View(new CProductWrap() { product = x });
        }
        [HttpPost]
        public IActionResult Edit(CProductWrap p)
        {
            DbuniPayContext db = new DbuniPayContext();
            Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == p.Pid);
            if (x != null)
            {
                x.Pname = p.Pname;
                x.Pprice = p.Pprice;
                x.Ptype = p.Ptype;
                x.Psize = p.Psize;
                x.Pcolor = p.Pcolor;
                x.Pdepiction = p.Pdepiction;
                x.Pcategory = p.Pcategory; 
                x.Pinventory = p.Pinventory;
                if (p.photoPath != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    x.Pimage = photoName;
                    p.photoPath.CopyTo(new FileStream(_enviro.WebRootPath + "/images/" + photoName, FileMode.Create));
                }
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
