using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspDotNetMVC1.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspDotNetMVC1.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult GetEmployeeList()
        {
            byte[] userId;
            if(HttpContext.Session.TryGetValue("UserID", out userId))
            {

                ViewBag.SessionSet = "true";
                return View("EmployeeList");
            }
            else
            {
                ViewBag.SessionSet = "false";
                return RedirectToAction("Index", "Home");
            }
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
