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
using System.Globalization;

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
        public IActionResult AddEmployee()
        {
            return View("AddEmployee", new StudentViewModel());
        }
        public IActionResult EditEmployee(int id)
        {
            string token = _session.GetString("token");
            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            var stud = studentRepoapi.GetStudentByID(id, token);
            StudentViewModel stdvm = new StudentViewModel();
            CultureInfo provider = CultureInfo.InvariantCulture;

            stdvm.ID = stud.Student.ID;
            stdvm.Name = stud.Student.Name;
            stdvm.Address = stud.Student.Address;
            stdvm.Role = stud.Student.Role;
            stdvm.Department = stud.Student.Department;
            stdvm.Email = stud.Student.Email;
            //std.DOB = stud.Student.DOB;
            //std.DOJ = stud.Student.DOJ;
            stdvm.DOB = stud.Student.DOB.ToString("MM/dd/yyyy");
            stdvm.DOJ = stud.Student.DOJ.ToString("MM/dd/yyyy");

            return View("AddEmployee", stdvm);
        }

        public IActionResult DelEmployee(int id)
        {
            StudentList studentlist = null;
            string token = _session.GetString("token");
            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            var stud = studentRepoapi.DelStudent(id, token);
            studentlist = studentRepoapi.GetStudents(token).Result;
            ViewBag.studentList = studentlist.Students;
            ViewBag.msg = "Employee deleted sucessfully (id:" + id + ")";
            return View("EmployeeList");
        }

        [HttpPost]
        public string SaveEmployee(StudentViewModel stud)
        {
            string status = string.Empty;
            string msg = string.Empty;
            if (stud.ID == 0)
            {
                string token = _session.GetString("token");
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (token != null)
                {
                    StudentRepoAPI studentRepoapi = new StudentRepoAPI();
                    WebApplication_Shared_Services.Model.Student std = new WebApplication_Shared_Services.Model.Student();
                    std.ID = stud.ID;
                    std.Name = stud.Name;
                    std.Address = stud.Address;
                    std.Role = stud.Role;
                    std.Department = stud.Department;
                    std.Email = stud.Email;
                    //std.DOB = stud.DOB;
                    //std.DOJ = stud.DOJ;
                    std.DOB = DateTime.ParseExact(stud.DOB.ToString(), "MM/dd/yyyy", provider);
                    std.DOJ = DateTime.ParseExact(stud.DOJ.ToString(), "MM/dd/yyyy", provider);

                    status = studentRepoapi.AddStudent(std, token).Result;
                }
                if (status == "200")
                {
                    msg = "Employee added successfully";
                    return msg;
                }
                else if (status == "401")
                {
                    msg = "Unauthorized access";
                    return msg;
                }
                else
                {
                    msg = "Something wen wrong in the application, try aftersometimes";
                    return msg;
                }
            }
            else
            {
                string token = _session.GetString("token");
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (token != null)
                {
                    StudentRepoAPI studentRepoapi = new StudentRepoAPI();
                    WebApplication_Shared_Services.Model.Student std = new WebApplication_Shared_Services.Model.Student();
                    std.ID = stud.ID;
                    std.Name = stud.Name;
                    std.Address = stud.Address;
                    std.Role = stud.Role;
                    std.Department = stud.Department;
                    std.Email = stud.Email;
                    //std.DOB = stud.DOB;
                    //std.DOJ = stud.DOJ;
                    stud.DOB = stud.DOB.Replace('-', '/');
                    stud.DOJ = stud.DOJ.Replace('-', '/');
                    std.DOB = DateTime.ParseExact(stud.DOB.ToString(), "MM/dd/yyyy", provider);
                    std.DOJ = DateTime.ParseExact(stud.DOJ.ToString(), "MM/dd/yyyy", provider);

                    status = studentRepoapi.UpdateStudent(std, token).Result;
                }
                if (status == "200")
                {
                    msg = "Employee updated successfully";
                    return msg;
                }
                else if (status == "401")
                {
                    msg = "Unauthorized access";
                    return msg;
                }
                else
                {
                    msg = "Something wen wrong in the application, try aftersometimes";
                    return msg;
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
