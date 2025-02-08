using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DTO;
using Project.Models;

namespace Project.Controllers
{
    public class AjaxController : Controller
    {
        private readonly DbuniPayContext _context;

        public AjaxController(DbuniPayContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IEnumerable<CustomCartDTO>> GetCartItems([FromBody] object request)
        {
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(json))
            {
                return null;  
            }

            var member = JsonSerializer.Deserialize<Tmember>(json);
            Console.WriteLine($"Member MID: {member.Mid}");   

            var cartItems = await _context.Tcarts
                .Where(c => c.Mid == member.Mid)
                .Select(c => new CustomCartDTO
                {
                    Id=c.Id,
                    PName = c.Pname,
                    PType = c.Ptype,
                    PCategory = c.Pcategory,
                    PCount = c.Pcount,
                    PSize = c.Psize,
                    PColor = c.Pcolor,
                    CustomText0 = c.CustomText0,
                    CustomText1 = c.CustomText1,
                    CustomPhoto0 = c.CustomPhoto0,
                    CustomPhoto1 = c.CustomPhoto1,
                    Photo0 = c.Photo0,
                    Photo1 = c.Photo1,
                    PPrice = c.Pprice
                }).ToListAsync();

            Console.WriteLine($"Found {cartItems.Count} items.");  

            return cartItems;  
        } 
         
        [HttpPost]
        public async Task<string> PostFavorite([FromBody] FavoriteDTO favoriteDTO)
        {
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(json))
            {
                return "請先登入會員";
            }
            var member = JsonSerializer.Deserialize<Tmember>(json);
            var existFavorite = await _context.Tfavorites.FirstOrDefaultAsync(c => c.Mid == member.Mid && c.Pid == favoriteDTO.PId);
            if (existFavorite == null)
            {
                var newFavorite = new Tfavorite
                {
                    Mid = member.Mid,
                    Pid = favoriteDTO.PId,
                    Pname = favoriteDTO.PName,
                    Pphoto = favoriteDTO.PPhoto,
                    Pprice = favoriteDTO.PPrice
                };
                _context.Tfavorites.Add(newFavorite);
                await _context.SaveChangesAsync();
                return "已加入收藏";
            }
            else
            {
                _context.Tfavorites.Remove(existFavorite);
                await _context.SaveChangesAsync();
                return "已移除收藏";
            }

        }

        [HttpGet]
        public async Task<List<Tfavorite>> GetFavorite() {
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(json))
            {
                return new List<Tfavorite>();
            }
            var member = JsonSerializer.Deserialize<Tmember>(json);

            var favorite = await _context.Tfavorites.Where(c => c.Mid == member.Mid)
                                                    .OrderByDescending(c=>c.FcreateDate)
                                                    .ToListAsync();
			Console.WriteLine($"Favorite count: {favorite.Count}");
			return favorite;
        }   

        [HttpGet]
        public async Task<IActionResult> GetFavoriteID() {
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (json == null)
            {
                return Json(new List<int>());
            }
            var member = JsonSerializer.Deserialize<Tmember>(json);
            

            var favoriteID = await _context.Tfavorites.Where(c => c.Mid == member.Mid)
                                                      .Select(c => c.Pid)
                                                      .ToListAsync();
            return Json(favoriteID);
        }

        [HttpGet]
        public async Task<IActionResult> Top4Sales()
        {
            var Top4Sales = await (from tod in _context.TorderDetails
                                   join td in _context.Torders on tod.Oid equals td.Oid
                                   where td.Odate >= new DateTime(2024, 1, 1) && td.Odate <= new DateTime(2024, 12, 31)
                                   group tod by tod.Pid into grouped
                                   select new
                                   {
                                       id = grouped.Key,
                                       pid = grouped.FirstOrDefault().Pid,
                                       count = grouped.Sum(t => t.Pcounts),
                                       pphoto = grouped.FirstOrDefault().Cimage,
                                       pname = grouped.FirstOrDefault().Pname,
                                       totalprice = grouped.Sum(t => t.Pprice * t.Pcounts),
                                       pprice = grouped.FirstOrDefault().Pprice
                                   })
                                   .OrderByDescending(x => x.count)
                                   .Take(4)
                                   .ToListAsync();
            return Json(Top4Sales);
        }

        [HttpGet]
        public async Task<IActionResult> New4Sales() {
            var Newest4 = await  _context.Tproducts.OrderByDescending(x => x.PcreatedDate).Take(4).ToListAsync();
            return Json(Newest4);
        }

        [HttpPost]
        public async Task<string> SaveToCart([FromBody] CustomCartDTO cart)
        {
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(json))
            {
                return "請先登入會員";
            }
            var member = JsonSerializer.Deserialize<Tmember>(json);
            var Cart = new Tcart
            {
                Mid=member.Mid,
                Pid=0,
                Pname=cart.PName,
                Ptype=cart.PType,
                Pcategory=cart.PCategory,
                Pcount=cart.PCount,
                Psize=cart.PSize,
                Pcolor=cart.PColor,
                CustomText0=cart.CustomText0,
                CustomText1 = cart.CustomText1,
                CustomPhoto0 =cart.CustomPhoto0,
                CustomPhoto1=cart.CustomPhoto1,
                Photo0= cart.Photo0,
                Photo1 = cart.Photo1,
                Pprice=cart.PPrice,
            };
            if (Cart.Photo0 == "") {
                return "請儲存正面圖案";
            } else if (Cart.Photo1 == "") {
                return "請儲存背面圖案";
            }
            _context.Tcarts.Add(Cart);
            await _context.SaveChangesAsync();
            return "已加入購物車";
        }

        [HttpPut]
        public async Task<string> UpdateItem(int productId, [FromBody] CustomCartDTO updatedCartItem)
        { 
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(json))
            {
                return "請先登入會員";
            }
             
            var member = JsonSerializer.Deserialize<Tmember>(json);
             
            var cartItem = await _context.Tcarts
                                         .Where(c => c.Mid == member.Mid && c.Pid == productId)
                                         .FirstOrDefaultAsync();
             
            if (cartItem == null)
            {
                return "商品不存在";
            }
             
            cartItem.Pcount = updatedCartItem.PCount;  // 修改数量 
              
            _context.Tcarts.Update(cartItem);
            await _context.SaveChangesAsync();

            return "商品已更新";
        }




        [HttpDelete]
        public async Task<string> DeleteItem(int productId)
        { 
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGEDIN_USER);
            if (string.IsNullOrEmpty(json))
            {
                return "請先登入會員";
            }
            
            var member = JsonSerializer.Deserialize<Tmember>(json);
             
            var cartItem = await _context.Tcarts
                             .Where(c => c.Mid == member.Mid && c.Pid == productId)  // 这里应该使用 Pid
                             .FirstOrDefaultAsync();  // 注意这里是 cartItem.Id == productId
             
            if (cartItem == null)
            { 
                return "商品不存在"+ productId;
            }
             
            _context.Tcarts.Remove(cartItem);
            await _context.SaveChangesAsync();

            return "已刪除商品";
        }


    }
}
