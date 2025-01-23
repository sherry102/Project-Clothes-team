using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class FrontHomeController : Controller
    {
        public IActionResult Customize()
        {
            return View();
        }
        public IActionResult ChangeClothes()
        {
            return View();
        }

		public IActionResult FrontIndex()
		{
            DbuniPayContext db = new DbuniPayContext();
            ViewBag.SalesCount_TOP4_Year = (from tod in db.TorderDetails
                                            join td in db.Torders on tod.Oid equals td.Oid
                                            where td.Odate >= new DateTime(2024, 1, 1) && td.Odate <= new DateTime(2024, 12, 31)
                                            group tod by tod.Pid into grouped
                                            select new
                                            {
                                                pid = grouped.Key,
                                                count = grouped.Sum(t => t.Pcounts),
                                                image = grouped.FirstOrDefault().Cimage,
                                                name = grouped.FirstOrDefault().Pname,
                                                totalprice = grouped.Sum(t => t.Pprice * t.Pcounts),
                                                price = grouped.FirstOrDefault().Pprice
                                            })
                                            .OrderByDescending(x => x.count)
                                            .Take(4)
                                            .ToList();
            return View();
		}
	}
}
