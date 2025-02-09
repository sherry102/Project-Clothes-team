using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DTO;
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
            return View();
		}

        public IActionResult Favorite() 
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult CartToPay()
        { 
            return View();
        }

        public IActionResult Advice() {
            return View();
        }

        public IActionResult Coupon()
        {
            return View();
        }

    }
}
