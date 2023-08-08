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
using AspDotNetMVC1.SharedService;
using System.IO;
using WebApplication_Shared_Services.Model;

namespace AspDotNetMVC1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStudentRepoAPI _studentRepoAPI;
        private readonly ILoggers _logger;
        private ISession _session;

        public EmployeeController(IHttpContextAccessor httpContextAccessor, ILoggers logger, IStudentRepoAPI studentRepoAPI)
        {
            var path = Directory.GetCurrentDirectory();
            _httpContextAccessor = httpContextAccessor;
            this._session = httpContextAccessor.HttpContext.Session;
            _studentRepoAPI = studentRepoAPI;
            _logger = logger;
            _logger.setFileLog($"{path}\\logs\\logger_" + DateTime.Now.ToString("dd_MM_yy") + ".txt");
        }
        public void TestSet()
        {
            _session.SetString("Test", "Ben Rules!");
        }

        public void TestGet()
        {
            var message = _session.GetString("Test");
        }

        public SessionStatus CheckSessionAndUserRole()
        {
            SessionStatus sessionState = new SessionStatus();
            if (HttpContext.Session.GetString("UserID") != null)
            {
                sessionState.token = _session.GetString("token");
                if (sessionState.token != null)
                {
                    sessionState.userrole = _studentRepoAPI.GetMyRole(_session.GetString("UserID"), sessionState.token);
                    if (sessionState.userrole.status == "200")
                    {
                        sessionState.IsValidUser = true;
                    }
                    else
                    {
                        sessionState.SessionSet = true;
                    }
                }
                else
                {
                    sessionState.Emsg = "Unauthorized access/Session expired";
                }
            }
            else
            {
                sessionState.Emsg = "Unauthorized access/Session expired";
            }
            return sessionState;
        }

        public IActionResult Get_Pager(Pager pager)
        {
            try
            {
                var sessionstate = CheckSessionAndUserRole();
                var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
                if (sessionstate.IsValidUser == true && availRole != null)
                {
                    //_logger.WriteLog("Log Started" + pager.ToString());
                    if (pager.currentPage > pager.pageCount) pager.currentPage = pager.pageCount;
                    else if (pager.currentPage < 1) pager.currentPage = 1;
                    ViewBag.pager = pager;
                    return PartialView("_Pager", pager);
                }
                else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
                {
                    ViewBag.SessionSet = "true";
                    return View("StudentHome");
                }
                else
                {
                    ViewBag.SessionSet = "false";
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return View("../Home/Index");
                }
            }
            catch (Exception ex)
            {
                //_logger.WriteLog(ex.Message + ex.StackTrace.ToString() + ex.InnerException.ToString());
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");

            }
        }


        public IActionResult GetEmpList(Pager pager)
        {
            byte[] userId;
            StudentList studentlist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                studentlist = _studentRepoAPI.GetStudentsPerPage(pager, _session.GetString("token"));

                //var queryResultPage = studentlist.Skip(pager.pageSize * (pager.currentPage - 1)).Take(pager.pageSize);

                ViewBag.studentList = studentlist.Students;
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
                    ViewBag.pager = new Pager() { pageSize = 10, currentPage = 1, pageCount = 13 };
                    ViewBag.currentPage = pager.currentPage;
                    return PartialView("_EmpList");
                }
                else if (studentlist.status == "401")
                {
                    ViewBag.SessionSet = "true";
                    return View("StudentHome");
                }
                else
                {
                    ViewBag.SessionSet = "false";
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }
        }


        [HttpGet]
        public FileContentResult GetthisStudentFile(int studid, string fileName)
        {
            byte[] fileBytes = null;
            Stream stream = null;
            StudentList studentlist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                fileBytes = _studentRepoAPI.GetthisStudentFile(studid, fileName, _session.GetString("token"));
                string contentType = "application/octet-stream";

                return new FileContentResult(fileBytes, contentType);
            }
            return null;
        }


        [HttpPost]
        public List<string> GetStudentFileListByID(int studid)
        {
            StudentList studentlist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                var res = _studentRepoAPI.GetStudentFileListByID(studid, _session.GetString("token"));
                return res;
            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return new List<string> { "Unauthorized access/Session expired" };
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return new List<string> { "Unauthorized access/Session expired" };
            }

        }
        public IActionResult GetDashboard()
        {
            byte[] userId;
            Pager pager = new Pager() { pageSize = 10, currentPage = 1, pageCount = -1 };
            StudentList studentlist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                if (pager.pageCount == -1) pager = _studentRepoAPI.GetStudentPageCount(pager, _session.GetString("token"));
                studentlist = _studentRepoAPI.GetStudentsPerPage(pager, _session.GetString("token"));
                ViewBag.studentList = studentlist.Students;
                if (studentlist.status == "200")
                {
                    ViewBag.SessionSet = "true";
                    ViewBag.pager = pager;
                    ViewBag.currentPage = pager.currentPage;
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
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }

        }
        public IActionResult AddEmployee()
        {

            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                return View("AddEmployee", new StudentViewModel());

            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }

        }
        public IActionResult EditEmployee(int id)
        {
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                var stud = _studentRepoAPI.GetStudentByID(id, _session.GetString("token"));
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
                stdvm.SkillSets = stud.Student.SkillSets;
                //stdvm.IsFileContainerExist = stud.Student.IsFileContainerExist;
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
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }



        }
        public IActionResult ViewEmployee(int id)
        {
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                var stud = _studentRepoAPI.GetStudentByID(id, _session.GetString("token"));
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
                stdvm.SkillSets = stud.Student.SkillSets;
                //stdvm.IsFileContainerExist = stud.Student.IsFileContainerExist;
                if (stud.status == "200")
                {
                    return View("ViewEmployee", stdvm);
                }
                else
                {
                    ViewBag.Emsg = "Unauthorized access/Session expired";
                    return View("../Home/Index");
                }
            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }



        }

        public IActionResult DelEmployee(int id)
        {

            Pager pager = new Pager() { pageSize = 10, currentPage = 1, pageCount = -1 };

            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                StudentList studentlist = null;
                var stud = _studentRepoAPI.DelStudent(id, sessionstate.token);
                if (pager.pageCount == -1) pager = _studentRepoAPI.GetStudentPageCount(pager, sessionstate.token);

                studentlist = _studentRepoAPI.GetStudentsPerPage(pager, sessionstate.token);
                ViewBag.studentList = studentlist.Students;

                ViewBag.pager = pager;
                ViewBag.currentPage = pager.currentPage;
                ViewBag.msg = "Employee deleted sucessfully (id:" + id + ")";
                return View("EmployeeList");
            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("StudentHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }

        }

        [HttpPost]
        public string SaveEmployee(StudentViewModel stud)
        {
            StudentProfile stdPrfo = new StudentProfile();
            string status = string.Empty;
            string msg = string.Empty;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
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
                        std.SkillSets = stud.SkillSets;
                        //std.IsFileContainerExist = stud.IsFileContainerExist; 
                        stdPrfo.profileFile = stud.profileFile;
                        status = _studentRepoAPI.AddStudent(std, token);
                        if (status.Contains("200:"))
                        {
                            var statusval = status.Split(':');
                            int.TryParse(statusval[1], out int idval);
                            if (stdPrfo?.profileFile?.Count > 0)
                            {
                                _studentRepoAPI.AddStudentProfile(idval, stdPrfo.profileFile, token);
                            }
                            status = statusval[0];
                        }
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
                        std.SkillSets = stud.SkillSets;
                        //std.IsFileContainerExist = stud.IsFileContainerExist;
                        stdPrfo.profileFile = stud.profileFile;
                        status = _studentRepoAPI.UpdateStudent(std, token);
                        if (status == "200" && stdPrfo?.profileFile?.Count > 0)
                        {
                            _studentRepoAPI.AddStudentProfile(std.ID, stdPrfo.profileFile, token);
                        }

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
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                msg = "Unauthorized access/Session expired";
                return msg;
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                msg = "Unauthorized access/Session expired";
                return msg;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
