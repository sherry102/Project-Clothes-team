using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModel;

namespace Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly DbuniPayContext _context;

        IWebHostEnvironment _enviro = null;
        public ProductController(IWebHostEnvironment p, DbuniPayContext context)
        {
            _enviro = p;
            _context = context;
        }

        [HttpGet]
        public async Task<List<Tproduct>> Getproduct()
        {
            var product = await _context.Tproducts.ToListAsync();
            return product;
        }

        public IActionResult List(ProductViewModel vm, int id)
        {
            DbuniPayContext db = new DbuniPayContext();
            string keyword = vm.txtKeyword;
            IEnumerable<Tproduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = db.Tproducts
                        .Where(t => !t.PisHided);  // 過濾已刪除的記錄
            else
                datas = db.Tproducts.Where(t => t.Pdescription.Contains(keyword));
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
                    x.PisHided = true;
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
            if (p.photoPath == null)
            {
                // 返回視圖，並顯示錯誤訊息
                ModelState.AddModelError("photoPath", "必須上傳照片。");
                return View(p);
            }

            string photoName = Guid.NewGuid().ToString() + ".jpg";
            p.Pphoto = photoName;
            using (var fileStream = new FileStream(
                    Path.Combine(_enviro.WebRootPath, "images", photoName),
                    FileMode.Create))
            {
                p.photoPath.CopyTo(fileStream);
            }

            DbuniPayContext db = new DbuniPayContext();
            p.PcreatedDate = DateTime.Now;
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
                x.Pdescription = p.Pdescription;
                x.Pcategory = p.Pcategory;     
                x.PcreatedDate = DateTime.Now;
                x.Pinventory = p.Pinventory;

                if (p.photoPath != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    x.Pphoto = photoName;
                    p.photoPath.CopyTo(new FileStream(_enviro.WebRootPath + "/images/" + photoName, FileMode.Create));
                }
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        // 重新上架頁面
        public IActionResult Renew(ProductViewModel vm)
        { 
            DbuniPayContext db = new DbuniPayContext();
            string keyword = vm.txtKeyword;
            IEnumerable<Tproduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = db.Tproducts
                        .Where(t => t.PisHided);  // 已刪除的記錄
            else
                datas = db.Tproducts.Where(t => t.Pdescription.Contains(keyword));
            List<CProductWrap> list = new List<CProductWrap>();
            foreach (var t in datas)
                list.Add(new CProductWrap() { product = t });
            return View(list);
        }

       

        // 重新上架
        public IActionResult Recovery(int id)
        {
            if (id != null)
            {
                DbuniPayContext db = new DbuniPayContext();
                Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == id);
                if (x != null)
                {
                    x.PisHided = false;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        // 刪除商品
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                DbuniPayContext db = new DbuniPayContext();
                Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == id);
                if (x != null)
                {
                    db.Tproducts.Remove(x);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Renew");
        }
    }
}
