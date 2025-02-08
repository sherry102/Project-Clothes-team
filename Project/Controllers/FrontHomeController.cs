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

        public IActionResult Search(string KeyWord) {
            if (string.IsNullOrEmpty(KeyWord)) {
                return View();
            }
            ViewBag.KeyWord = KeyWord;
            return View();
        }
	}
}
