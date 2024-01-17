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
        private readonly IEmployeeRepoAPI _EmployeeRepoAPI;
        private readonly ILoggers _logger;
        private ISession _session;

        public EmployeeController(IHttpContextAccessor httpContextAccessor, ILoggers logger, IEmployeeRepoAPI EmployeeRepoAPI)
        {
            var path = Directory.GetCurrentDirectory();
            _httpContextAccessor = httpContextAccessor;
            this._session = httpContextAccessor.HttpContext.Session;
            _EmployeeRepoAPI = EmployeeRepoAPI;
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
                    sessionState.userrole = _EmployeeRepoAPI.GetMyRole(_session.GetString("UserID"), sessionState.token);
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
                    return View("EmployeeHome");
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
            EmployeeList Employeelist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                Employeelist = _EmployeeRepoAPI.GetEmployeesPerPage(pager, _session.GetString("token"));

                //var queryResultPage = Employeelist.Skip(pager.pageSize * (pager.currentPage - 1)).Take(pager.pageSize);

                ViewBag.EmployeeList = Employeelist.Employees;
                ViewBag.currentPager = pager;
                if (Employeelist.status == "200")
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
                    ViewBag.pager = pager;
                    ViewBag.currentPage = pager.currentPage;
                    return PartialView("_EmpList");
                }
                else if (Employeelist.status == "401")
                {
                    ViewBag.SessionSet = "true";
                    return View("EmployeeHome");
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
                return View("EmployeeHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }
        }


        [HttpGet]
        public FileContentResult GetthisEmployeeFile(int emplid, string fileName)
        {
            byte[] fileBytes = null;
            Stream stream = null;
            EmployeeList Employeelist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                fileBytes = _EmployeeRepoAPI.GetthisEmployeeFile(emplid, fileName, _session.GetString("token"));
                string contentType = "application/octet-stream";

                return new FileContentResult(fileBytes, contentType);
            }
            return null;
        }


        [HttpPost]
        public List<string> GetEmployeeFileListByID(int emplid)
        {
            EmployeeList Employeelist = null;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                var res = _EmployeeRepoAPI.GetEmployeeFileListByID(emplid, _session.GetString("token"));
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
        public IActionResult GetDashboard(Pager pager)
        {
            byte[] userId;
            EmployeeList Employeelist = null;
            if (pager == null || pager.pageSize == 0) pager = new Pager() { pageSize = 10, currentPage = 1, pageCount = -1 };
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null)
            {
                if (pager.pageCount == -1) pager = _EmployeeRepoAPI.GetEmployeePageCount(pager, _session.GetString("token"));
                Employeelist = _EmployeeRepoAPI.GetEmployeesPerPage(pager, _session.GetString("token"));
                ViewBag.EmployeeList = Employeelist.Employees;
                if (Employeelist.status == "200")
                {
                    ViewBag.SessionSet = "true";
                    ViewBag.pager = pager;
                    ViewBag.currentPage = pager.currentPage;
                    return View("EmployeeList");
                }
                else if (Employeelist.status == "401")
                {
                    ViewBag.SessionSet = "true";
                    return View("EmployeeHome");
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
                return View("EmployeeHome");
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
                return View("AddEmployee", new EmployeeViewModel());

            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("EmployeeHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }

        }
        public IActionResult EditEmployee(int id, Pager currentPager)
        {
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                var empl = _EmployeeRepoAPI.GetEmployeeByID(id, _session.GetString("token"));
                EmployeeViewModel emplvm = new EmployeeViewModel();
                CultureInfo provider = CultureInfo.InvariantCulture;

                emplvm.ID = empl.Employee.ID;
                emplvm.Name = empl.Employee.Name;
                emplvm.Address = empl.Employee.Address;
                emplvm.Role = empl.Employee.Role;
                emplvm.Department = empl.Employee.Department;
                emplvm.Email = empl.Employee.Email;
                //emp.DOB = empl.Employee.DOB;
                //emp.DOJ = empl.Employee.DOJ;
                emplvm.DOB = empl.Employee.DOB.ToString("MM/dd/yyyy");
                emplvm.DOJ = empl.Employee.DOJ.ToString("MM/dd/yyyy");
                emplvm.SkillSets = empl.Employee.SkillSets;
                ViewBag.pager = currentPager;
                //emplvm.IsFileContainerExist = empl.Employee.IsFileContainerExist;
                if (empl.status == "200")
                {
                    return View("AddEmployee", emplvm);
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
                return View("EmployeeHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }



        }
        public IActionResult ViewEmployee(int id, Pager currentPager)
        {
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                var empl = _EmployeeRepoAPI.GetEmployeeByID(id, _session.GetString("token"));
                EmployeeViewModel emplvm = new EmployeeViewModel();
                CultureInfo provider = CultureInfo.InvariantCulture;

                emplvm.ID = empl.Employee.ID;
                emplvm.Name = empl.Employee.Name;
                emplvm.Address = empl.Employee.Address;
                emplvm.Role = empl.Employee.Role;
                emplvm.Department = empl.Employee.Department;
                emplvm.Email = empl.Employee.Email;
                //emp.DOB = empl.Employee.DOB;
                //emp.DOJ = empl.Employee.DOJ;
                emplvm.DOB = empl.Employee.DOB.ToString("MM/dd/yyyy");
                emplvm.DOJ = empl.Employee.DOJ.ToString("MM/dd/yyyy");
                emplvm.SkillSets = empl.Employee.SkillSets;
                //emplvm.IsFileContainerExist = empl.Employee.IsFileContainerExist;
                ViewBag.pager = currentPager;
                if (empl.status == "200")
                {
                    return PartialView("ViewEmployee", emplvm);
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
                return View("EmployeeHome");
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
                EmployeeList Employeelist = null;
                var empl = _EmployeeRepoAPI.DelEmployee(id, sessionstate.token);
                if (pager.pageCount == -1) pager = _EmployeeRepoAPI.GetEmployeePageCount(pager, sessionstate.token);

                Employeelist = _EmployeeRepoAPI.GetEmployeesPerPage(pager, sessionstate.token);
                ViewBag.EmployeeList = Employeelist.Employees;

                ViewBag.pager = pager;
                ViewBag.currentPage = pager.currentPage;
                ViewBag.msg = "Employee deleted sucessfully (id:" + id + ")";
                return View("EmployeeList");
            }
            else if (sessionstate.IsValidUser == false && sessionstate.SessionSet == true)
            {
                ViewBag.SessionSet = "true";
                return View("EmployeeHome");
            }
            else
            {
                ViewBag.SessionSet = "false";
                ViewBag.Emsg = "Unauthorized access/Session expired";
                return View("../Home/Index");
            }

        }

        [HttpPost]
        public string SaveEmployee(EmployeeViewModel empl)
        {
            EmployeeProfile empPrfo = new EmployeeProfile();
            string status = string.Empty;
            string msg = string.Empty;
            var sessionstate = CheckSessionAndUserRole();
            var availRole = sessionstate.userrole?.userRole.Where(u => u.roleID == 2 || u.roleID == 3 || u.roleID == 4 || u.roleID == 5).ToList();
            if (sessionstate.IsValidUser == true && availRole != null && availRole.Count > 0)
            {
                if (empl.ID == 0)
                {
                    string token = _session.GetString("token");
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (token != null)
                    {
                        WebApplication_Shared_Services.Model.Employee emp = new WebApplication_Shared_Services.Model.Employee();
                        emp.ID = empl.ID;
                        emp.Name = empl.Name;
                        emp.Address = empl.Address;
                        emp.Role = empl.Role;
                        emp.Department = empl.Department;
                        emp.Email = empl.Email;
                        //emp.DOB = empl.DOB;
                        //emp.DOJ = empl.DOJ;
                        emp.DOB = DateTime.ParseExact(empl.DOB.ToString(), "MM/dd/yyyy", provider);
                        emp.DOJ = DateTime.ParseExact(empl.DOJ.ToString(), "MM/dd/yyyy", provider);
                        emp.SkillSets = empl.SkillSets;
                        //emp.IsFileContainerExist = empl.IsFileContainerExist; 
                        empPrfo.profileFile = empl.profileFile;
                        status = _EmployeeRepoAPI.AddEmployee(emp, token);
                        if (status.Contains("200:"))
                        {
                            var statusval = status.Split(':');
                            int.TryParse(statusval[1], out int idval);
                            if (empPrfo?.profileFile?.Count > 0)
                            {
                                _EmployeeRepoAPI.AddEmployeeProfile(idval, empPrfo.profileFile, token);
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
                        WebApplication_Shared_Services.Model.Employee emp = new WebApplication_Shared_Services.Model.Employee();
                        emp.ID = empl.ID;
                        emp.Name = empl.Name;
                        emp.Address = empl.Address;
                        emp.Role = empl.Role;
                        emp.Department = empl.Department;
                        emp.Email = empl.Email;
                        //emp.DOB = empl.DOB;
                        //emp.DOJ = empl.DOJ;
                        empl.DOB = empl.DOB.Replace('-', '/');
                        empl.DOJ = empl.DOJ.Replace('-', '/');
                        emp.DOB = DateTime.ParseExact(empl.DOB.ToString(), "MM/dd/yyyy", provider);
                        emp.DOJ = DateTime.ParseExact(empl.DOJ.ToString(), "MM/dd/yyyy", provider);
                        emp.SkillSets = empl.SkillSets;
                        //emp.IsFileContainerExist = empl.IsFileContainerExist;
                        empPrfo.profileFile = empl.profileFile;
                        status = _EmployeeRepoAPI.UpdateEmployee(emp, token);
                        if (status == "200" && empPrfo?.profileFile?.Count > 0)
                        {
                            _EmployeeRepoAPI.AddEmployeeProfile(emp.ID, empPrfo.profileFile, token);
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
