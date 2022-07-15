using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspDotNetMVC1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AspDotNetMVC1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginRegisterModel loginModel)
        {

            if (loginModel.login.username == "test" && loginModel.login.password == "test")
            {
                ViewBag.NotValidUser = "false";
                HttpContext.Session.SetString("UserID", loginModel.login.username);

                return RedirectToAction("GetEmployeeList", "Employee");
            }
            else 

            {
                ViewBag.NotValidUser = "true";

            }

            return View("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
            [HttpPost]
        public IActionResult Regiser(Register register)
        {
            if (ModelState.IsValid)
            {
                if ("Success" == "Success")
                {
                    ViewBag.RegisteredUser = "true";
                }
                else
                {
                    ViewBag.RegisteredUser = "false";
                }
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult ForgetPassword(string email)
        {

            var item = "Success";
            if (item == "Success")
            {

                return RedirectToAction("GetEmployeeList", "Employee");
            }
            else if (item == "User Does not Exists")

            {
                ViewBag.NotValidUser = item;

            }
            else
            {
                ViewBag.Failedcount = item;
            }

            return View("Index");
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
