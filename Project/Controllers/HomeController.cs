using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Project.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			DbuniPayContext db = new DbuniPayContext();

			var today = DateTime.Today;
			var currentMonth = DateTime.Today.Month;
			var currentYear = DateTime.Today.Year;
			var lastDayOfMonth = DateTime.DaysInMonth(currentYear, currentMonth);

			List<Torder> order = db.Torders.ToList();
			List<TorderDetail> orderDetail = db.TorderDetails.ToList();
			List<Tproduct> product_none = db.Tproducts.Where(t => t.Pinventory == 0).ToList();
			List<Torder> order_none = db.Torders.Where(t => t.Ostatus == "待審核" && t.Odate <= today.AddDays(-3)).ToList();
			List<int> month_Days = new List<int>();
			List<int> members_count_Month = new List<int>();
			List<int> members_count_Year = new List<int>();
			List<int> product_inventory = new List<int>();
			List<string> product_name = new List<string>();
			List<int> type_sales_count = (from tod in db.TorderDetails
										  join tp in db.Tproducts on tod.Pid equals tp.Pid
										  group tod by tp.Pcategory into grouped
										  select grouped.Sum(t => t.Odcounts)
											)
											.ToList();

			int sumday = 0;
			int summonth = 0;

			for (int day = 1; day <= lastDayOfMonth; day++)
			{
				sumday += db.Tmembers.Count(t => t.McreatedDate.Month == currentMonth && t.McreatedDate.Year == currentYear && t.McreatedDate.Day == day);

				members_count_Month.Add(sumday);
				month_Days.Add(day);
			}

			for (int month = 1; month <= 12; month++)
			{
				summonth += db.Tmembers.Count(t => t.McreatedDate.Month == month && t.McreatedDate.Year == currentYear);
				members_count_Year.Add(summonth);
			}

			product_inventory.AddRange(db.Tproducts.Where(t => t.Pinventory <= 50).Select(t => t.Pinventory).ToList());
			product_name.AddRange(db.Tproducts.Where(t => t.Pinventory <= 50).Select(t => t.Pname).ToList());

			ViewBag.SalesCount_TOP5_Today = (from tod in db.TorderDetails
											 join td in db.Torders on tod.Oid equals td.Oid
											 where td.Odate.Date == today
											 group tod by tod.Pid into grouped
											 select new
											 {
												 pid = grouped.Key,
												 count = grouped.Sum(t => t.Odcounts),
												 image = grouped.FirstOrDefault().Cimage,
												 name = grouped.FirstOrDefault().Pname,
												 totalprice = grouped.Sum(t => t.Pprice * t.Odcounts),
												 price = grouped.FirstOrDefault().Pprice
											 })
											 .Take(5)
											 .ToList();

			ViewBag.SalesCount_TOP5_Month = (from tod in db.TorderDetails
											 join td in db.Torders on tod.Oid equals td.Oid
											 where td.Odate.Month == currentMonth && td.Odate.Year == currentYear
											 group tod by tod.Pid into grouped
											 select new
											 {
												 pid = grouped.Key,
												 count = grouped.Sum(t => t.Odcounts),
												 image = grouped.FirstOrDefault().Cimage,
												 name = grouped.FirstOrDefault().Pname,
												 totalprice = grouped.Sum(t => t.Pprice * t.Odcounts),
												 price = grouped.FirstOrDefault().Pprice
											 })
											 .Take(5)
											 .ToList();

			ViewBag.SalesCount_TOP5_Year = (from tod in db.TorderDetails
											join td in db.Torders on tod.Oid equals td.Oid
											where td.Odate >= new DateTime(2024, 1, 1) && td.Odate <= new DateTime(2024, 12, 31)
											group tod by tod.Pid into grouped
											select new
											{
												pid = grouped.Key,
												count = grouped.Sum(t => t.Odcounts),
												image = grouped.FirstOrDefault().Cimage,
												name = grouped.FirstOrDefault().Pname,
												totalprice = grouped.Sum(t => t.Pprice * t.Odcounts),
												price = grouped.FirstOrDefault().Pprice
											})
											  .Take(5)
											  .ToList();

			ViewBag.typesales_today = (from tod in db.TorderDetails
									   join tp in db.Tproducts on tod.Pid equals tp.Pid
									   join td in db.Torders on tod.Oid equals td.Oid
									   where td.Odate.Date == today
									   group new { tod, tp } by tp.Pcategory into grouped
									   select new
									   {
										   value = grouped.Sum(g => g.tod.Odcounts),
										   name = grouped.Key // 分組鍵即為 Pcategory
									   }
										)
										.ToList();

			ViewBag.typesales_month = (from tod in db.TorderDetails
									   join tp in db.Tproducts on tod.Pid equals tp.Pid
									   join td in db.Torders on tod.Oid equals td.Oid
									   where td.Odate.Month == currentMonth && td.Odate.Year == currentYear
									   group new { tod, tp } by tp.Pcategory into grouped
									   select new
									   {
										   value = grouped.Sum(g => g.tod.Odcounts),
										   name = grouped.Key // 分組鍵即為 Pcategory
									   }
										)
										.ToList();

			ViewBag.typesales_year = (from tod in db.TorderDetails
									   join tp in db.Tproducts on tod.Pid equals tp.Pid
									   join td in db.Torders on tod.Oid equals td.Oid
									   where td.Odate.Year == currentYear
									   group new { tod, tp } by tp.Pcategory into grouped
									   select new
									   {
										   value = grouped.Sum(g => g.tod.Odcounts),
										   name = grouped.Key // 分組鍵即為 Pcategory
									   }
										)
										.ToList();

			int totalprice_today = db.Torders.Where(t => t.Odate.Date == today).Sum(t => t.OtotalPrice);
			int totalprice_month = db.Torders.Where(t => t.Odate.Month == currentMonth && t.Odate.Year == currentYear).Sum(t => t.OtotalPrice);
			int totalprice_year = db.Torders.Where(t => t.Odate.Year == currentYear).Sum(t => t.OtotalPrice);

			int totalsales_today = db.Torders.Where(t => t.Odate.Date == today).Count();
			int totalsales_month = db.Torders.Where(t => t.Odate.Month == currentMonth && t.Odate.Year == currentYear).Count();
			int totalsales_year = db.Torders.Where(t => t.Odate.Year == currentYear).Count();


			var viewModel = new IndexViewModel
			{
				Torder = order,
				TorderDetail = orderDetail,
				TotalPrice_Today = totalprice_today,
				TotalPrice_Month = totalprice_month,
				TotalPrice_Year = totalprice_year,
				TotalSales_Today = totalsales_today,
				TotalSales_Month = totalsales_month,
				TotalSales_Year = totalsales_year,
				Torder_none = order_none,
				Tproduct_none = product_none,
				members_count_Month = members_count_Month,
				members_count_Year = members_count_Year,
				month_Days = month_Days,
				product_inventory = product_inventory,
				product_name = product_name,
			};
			return View(viewModel);
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(MemberViewModel m)
		{
			//DbuniPayContext db = new DbuniPayContext();
			//Tmember T = db.Tmembers.FirstOrDefault(c => c.Maccount == m.Account && c.Mpassword == m.Password);

			//if (T != null && T.Mpassword == m.Password) 
			//{
			//    string json = JsonSerializer.Serialize(T); 
			//    HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER,json);
			//    return RedirectToAction("Index");
			//}
			//return View();

			return RedirectToAction("Index");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
