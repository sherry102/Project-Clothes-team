using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DTO;
using Project.Models;
using Project.ViewModel;
using System.Security.Cryptography;

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

        //前台ProductList
        public IActionResult ProductList(string keyword, int id)
        {
            DbuniPayContext db = new DbuniPayContext();
            IEnumerable<Tproduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = db.Tproducts
                        .Where(t => !t.PisHided);  // 過濾已下架的記錄
            else
                datas = db.Tproducts.Where(t => t.Pname.Contains(keyword) || t.Ptype.Contains(keyword) || t.Pcategory.Contains(keyword));
            List<CProductWrap> ProductList = new List<CProductWrap>();
            foreach (var t in datas)
                ProductList.Add(new CProductWrap() { product = t });
            return View(ProductList);
        }

        //前台Productdetail
        public IActionResult ProductDetail(int? id)
        {
            if (id == null)
                return RedirectToAction("ProductList");

            DbuniPayContext db = new DbuniPayContext();
            Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == id);
            if (x == null)
                return RedirectToAction("ProductList");

            // 查詢對應的圖片列表
            List<string> images = db.Tpimages
                                     .Where(img => img.Pid == id)
                                     .Select(img => img.Piname) // 取得圖片名稱
                                     .ToList();

            return View(new CProductWrap()
            {
                product = x,
                Images = images
            });
        }

        //後台List
        public IActionResult List(ProductViewModel vm, int id)
        {
            DbuniPayContext db = new DbuniPayContext();
            string keyword = vm.txtKeyword;

            // 取得符合條件的產品 (排除隱藏的)
            var query = db.Tproducts
                          .Where(t => !t.PisHided);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.Pdescription.Contains(keyword));
            }

            // 直接用 LINQ Join 取得產品與庫存
            var productList = query
                .Select(t => new CProductWrap
                {
                    product = t,
                    TproductInventories = db.TproductInventories
                                   .Where(i => i.Pid == t.Pid)
                                   .ToList()
                })
                .ToList();

            return View(productList);
        }

        //後台商品下架
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
        //後台新增商品
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CProductWrap p, List<IFormFile> photos)
        {
            DbuniPayContext db = new DbuniPayContext();
            var transaction = db.Database.BeginTransaction();
            try
            {
                if (p.photoPath == null)
                {
                    ModelState.AddModelError("photoPath", "必須上傳照片。");
                    return View(p);
                }

                // 處理主圖上傳
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                p.Pphoto = photoName;
                using (var fileStream = new FileStream(
                    Path.Combine(_enviro.WebRootPath, "images", photoName),
                    FileMode.Create))
                {
                    p.photoPath.CopyTo(fileStream);
                }

                // 設置創建時間
                p.PcreatedDate = DateTime.Now;

                // 先新增產品，讓資料庫生成 Pid
                db.Tproducts.Add(p.product);
                db.SaveChanges();  // 確保 Pid 生成

                // 獲取剛剛插入的產品 ID
                int productId = p.product.Pid;

                // 處理多張圖片上傳 (存入 Tpimages)
                if (photos != null && photos.Count > 0)
                {
                    foreach (var photo in photos)
                    {
                        if (photo.Length > 0)
                        {
                            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                            string imagePath = Path.Combine(_enviro.WebRootPath, "images", imageName);

                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                photo.CopyTo(stream);
                            }

                            // 存入 Tpimage 表
                            Tpimage img = new Tpimage()
                            {
                                Pid = productId,   // 關聯產品 ID
                                Piname = imageName // 存圖片名稱
                            };
                            db.Tpimages.Add(img);
                        }
                    }
                }

                db.SaveChanges(); // 儲存所有圖片
                transaction.Commit(); // 交易提交

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // 發生錯誤，回滾交易
                ModelState.AddModelError("", "發生錯誤：" + ex.Message);
                return View(p);
            }
        }

        //後台編輯商品
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
        public IActionResult Edit(CProductWrap p, List<IFormFile> photos)
        {
            DbuniPayContext db = new DbuniPayContext();
            Tproduct x = db.Tproducts.FirstOrDefault(c => c.Pid == p.Pid);
            if (x != null)
            {
                x.Pname = p.Pname;
                x.Pprice = p.Pprice;
                x.Ptype = p.Ptype;
                x.Pdescription = p.Pdescription;
                x.Pcategory = p.Pcategory;
                x.PcreatedDate = DateTime.Now;
                x.Pinventory = p.Pinventory;

                //存首圖
                if (p.photoPath != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    x.Pphoto = photoName;
                    p.photoPath.CopyTo(new FileStream(_enviro.WebRootPath + "/images/" + photoName, FileMode.Create));
                }

                // 編輯多張圖片上傳 (存入 Tpimages)
                if (photos != null && photos.Count > 0)
                {
                    foreach (var photo in photos)
                    {
                        if (photo.Length > 0)
                        {
                            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                            string imagePath = Path.Combine(_enviro.WebRootPath, "images", imageName);

                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                photo.CopyTo(stream);
                            }
                            // 存入 Tpimage 表
                            Tpimage img = new Tpimage()
                            {
                                Pid = p.Pid,   // 關聯產品 ID
                                Piname = imageName // 存圖片名稱
                            };
                            db.Tpimages.Add(img);
                        }
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        // 後台重新上架頁面
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

        // 後台重新上架
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

        // 後台刪除商品
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

        public IActionResult Style()
        {
            return View();
        }
    }
}
