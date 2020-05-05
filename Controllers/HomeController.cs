using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CookieTest.Models;
using Microsoft.AspNetCore.Http;
using CookieTest.Dal;

namespace CookieTest.Controllers
{
    public class HomeController : Controller
    {
        // private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        // public HomeController(IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        // {
        //     _httpContextAccessor = httpContextAccessor;
        //     _dataContext = dataContext;
        // }
        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        // private readonly ILogger<HomeController> _logger;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                foreach (User dbUser in _dataContext.Users)
                {
                    if (user.Password == dbUser.Password && user.Username == dbUser.Username)
                    {
                        Response.Cookies.Append("username", dbUser.Username);
                        return RedirectToAction("UserDashboard");
                    }
                }
            }
            TempData["Message"] = "Invalid username or password";
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            if(Request.Cookies["username"] != null)
            {
                return RedirectToAction("UserDashboard");
            }
            return View();
        }

        public IActionResult UserDashboard()
        {
            if(Request.Cookies["username"] != null)
            {
                ViewData["username"] = Request.Cookies["username"];
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
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

        private string GetValueFromCookie(string key)
        {
            return Request.Cookies[key];
        }

        private void SetValueToCookie(string key, string value, int? expiredTime)
        {
            CookieOptions options = new CookieOptions();
            if (expiredTime.HasValue)
            {
                options.Expires = DateTime.Now.AddMinutes(expiredTime.Value);
            }
            else
            {
                options.Expires = DateTime.Now.AddMilliseconds(100);
            }
            Response.Cookies.Append(key, value, options);
        }

        private void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
