using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspDotNetMVC1.Models;
using Microsoft.AspNetCore.Authorization;
using AspDotNetMVC1.ConsumeAPI;
using Microsoft.AspNetCore.Http;

namespace AspDotNetMVC1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public EmployeeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void TestSet()
        {
            _session.SetString("Test", "Ben Rules!");
        }

        public void TestGet()
        {
            var message = _session.GetString("Test");
        }

        public IActionResult GetEmployeeList()
        {
            byte[] userId;
            StudentList studentlist = null;
            if (_session.GetString("UserID") != null)
            {
                string token = _session.GetString("token");
                if (token != null)
                {
                    StudentRepoAPI studentRepoapi = new StudentRepoAPI();
                    studentlist = studentRepoapi.GetStudents(token).Result;
                    ViewBag.studentList = studentlist.Students;
                }

            }
            if (studentlist.status == "200")
            {
                ViewBag.SessionSet = "true"; 
                return View("EmployeeList");
            }
            else if (studentlist.status == "401")
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
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
