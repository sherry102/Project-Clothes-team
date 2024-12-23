using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.ViewModel;
using System.Diagnostics;
using System.Text.Json;

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

            List<Torder> order = db.Torders.ToList();
            List<TorderDetail> orderDetail = db.TorderDetails.ToList();
			List<TorderDetail> SalesCount_TOP5 = db.TorderDetails.OrderByDescending(t => t.Odcounts).Take(5).ToList();

			int totalprice_today = db.Torders.Where(t=>t.Odate.Date==today).Sum(t => t.OtotalPrice);
            int totalprice_month = db.Torders.Where(t => t.Odate.Month==currentMonth && t.Odate.Year==currentYear).Sum(t => t.OtotalPrice);
            int totalprice_year = db.Torders.Where(t => t.Odate.Year == currentYear).Sum(t => t.OtotalPrice);

            int totalsales_today = db.Torders.Where(t => t.Odate.Date == today).Count();
            int totalsales_month = db.Torders.Where(t => t.Odate.Month == currentMonth && t.Odate.Year == currentYear).Count();
            int totalsales_year = db.Torders.Where(t => t.Odate.Year == currentYear).Count();

            var viewModel = new IndexViewModel {
            Torder=order,
            TorderDetail=orderDetail,
            TotalPrice_Today= totalprice_today,
            TotalPrice_Month=totalprice_month,
            TotalPrice_Year=totalprice_year,
            TotalSales_Today=totalsales_today,
            TotalSales_Month=totalsales_month,
            TotalSales_Year=totalsales_year,
            SalesCount_TOP5= SalesCount_TOP5
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
            DbuniPayContext db = new DbuniPayContext();
            Tmember T = db.Tmembers.FirstOrDefault(c => c.Maccount == m.Account && c.Mpassword == m.Password);

            if (T != null && T.Mpassword == m.Password) 
            {
                string json = JsonSerializer.Serialize(T); 
                HttpContext.Session.SetString(CDictionary.SK_LOGEDIN_USER,json);
                return RedirectToAction("Index");
            }
            return View();
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
