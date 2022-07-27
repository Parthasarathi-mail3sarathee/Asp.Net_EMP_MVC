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
        public IActionResult Get_Pager(Pager pager)
        {

            if (pager.currentPage > pager.pageCount) pager.currentPage = pager.pageCount;
            else if (pager.currentPage < 1) pager.currentPage = 1;
            return View("_Pager", pager);
        }



        public IActionResult GetDashboard()
        {
            byte[] userId;
            StudentList studentlist = null;
            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            if (_session.GetString("UserID") != null)
            {
                string token = _session.GetString("token");
                if (token != null)
                {
                    var userrole = studentRepoapi.GetMyRole(_session.GetString("UserID"), token).Result;
                    if (userrole.status == "200")
                    {
                        var availRole = userrole.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
                        if (availRole != null)
                        {
                            studentlist = studentRepoapi.GetStudents(token).Result;
                            ViewBag.studentList = studentlist.Students;
                        }
                        else
                        {
                            ViewBag.SessionSet = "true";
                            return View("StudentHome");

                        }
                    }
                    else
                    {
                        ViewBag.SessionSet = "true";
                        return View("StudentHome");

                    }
                }
                else
                {
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return View("../Home/Index");
                }
            }
            else
            {
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }
            if (studentlist.status == "200")
            {
                ViewBag.SessionSet = "true";

                // Stored procedure for fetching recoreds for particular page with minimum data
                //DECLARE @PageNumber AS INT
                //DECLARE @RowsOfPage AS INT
                //SET @PageNumber = 2
                //SET @RowsOfPage = 4
                //SELECT FruitName, Price FROM SampleFruits
                //ORDER BY Price
                //OFFSET(@PageNumber - 1) * @RowsOfPage ROWS
                //FETCH NEXT @RowsOfPage ROWS ONLY
                ViewBag.pager = new Pager() { pageSize = 10, currentPage = 50, pageCount = 50 };
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

            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            if (_session.GetString("UserID") != null)
            {
                string token = _session.GetString("token");
                if (token != null)
                {
                    var userrole = studentRepoapi.GetMyRole(_session.GetString("UserID"), token).Result;
                    if (userrole.status == "200")
                    {
                        var availRole = userrole.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
                        if (availRole != null && availRole.Count > 0)
                        {
                            return View("AddEmployee", new StudentViewModel());
                        }
                        else
                        {
                            ViewBag.SessionSet = "true";
                            return View("StudentHome");

                        }
                    }
                    else
                    {
                        ViewBag.SessionSet = "true";
                        return View("StudentHome");

                    }

                }
                else
                {
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return View("../Home/Index");
                }
            }
            else
            {
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }
        }
        public IActionResult EditEmployee(int id)
        {
            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            if (_session.GetString("UserID") != null)
            {
                string token = _session.GetString("token");
                if (token != null)
                {
                    var userrole = studentRepoapi.GetMyRole(_session.GetString("UserID"), token).Result;
                    if (userrole.status == "200")
                    {
                        var availRole = userrole.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
                        if (availRole != null && availRole.Count > 0)
                        {
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
                            if (stud.status == "200")
                            {
                                return View("AddEmployee", stdvm);
                            }
                            else
                            {
                                ViewBag.Emsg = "Unauthorized access/Session expired";
                                return View("../Home/Index");
                            }
                        }
                        else
                        {
                            ViewBag.SessionSet = "true";
                            return View("StudentHome");

                        }
                    }
                    else
                    {
                        ViewBag.SessionSet = "true";
                        return View("StudentHome");

                    }
                }
                else
                {
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return View("../Home/Index");
                }
            }
            else
            {
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }
        }

        public IActionResult DelEmployee(int id)
        {
            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            if (_session.GetString("UserID") != null)
            {
                string token = _session.GetString("token");
                if (token != null)
                {
                    var userrole = studentRepoapi.GetMyRole(_session.GetString("UserID"), token).Result;
                    if (userrole.status == "200")
                    {
                        var availRole = userrole.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
                        if (availRole != null && availRole.Count > 0)
                        {
                            StudentList studentlist = null;
                            var stud = studentRepoapi.DelStudent(id, token);
                            studentlist = studentRepoapi.GetStudents(token).Result;
                            ViewBag.studentList = studentlist.Students;

                            ViewBag.pager = new Pager() { pageSize = 10, currentPage = 50, pageCount = 50 };
                            ViewBag.msg = "Employee deleted sucessfully (id:" + id + ")";
                            return View("EmployeeList");
                        }
                        else
                        {
                            ViewBag.SessionSet = "true";
                            return View("StudentHome");

                        }
                    }
                    else
                    {
                        ViewBag.Emsg = "Unauthorized access/Session expired";
                        return View("../Home/Index");
                    }
                }
                else
                {
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return View("../Home/Index");
                }
            }
            else
            {
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }
        }

        [HttpPost]
        public string SaveEmployee(StudentViewModel stud)
        {
            StudentRepoAPI studentRepoapi = new StudentRepoAPI();
            string status = string.Empty;
            string msg = string.Empty;
            if (stud.ID == 0)
            {
                string token = _session.GetString("token");
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (token != null)
                {
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
