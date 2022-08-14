using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_Shared_Services.Model;

namespace AspDotNetMVC1.Models
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Register
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }

    public class forgetemail
    {
        public string email { get; set; }
    }

    public class LoginRegisterModel
    {
        public Login login { get; set; }
        public Register register { get; set; }
        public string email { get; set; }

    }

    public class Token
    {
        public string token { get; set; }
    }

    public class StudentList
    {
        public List<Student> Students { get; set; }
        public string status { get; set; }
    }
    public class UserRoleModel
    {
        public List<UserRole> userRole { get; set; }
        public string status { get; set; }
    }


    public class StudentModel
    {
        public Student Student { get; set; }
        public string status { get; set; }
    }
    public class StudentViewModel
    {
        [StringLength(20), Required]
        public string Name { get; set; }
        public int ID { get; set; }
        [StringLength(300)]
        public string Address { get; set; }
        [StringLength(7)]
        public string Role { get; set; }
        [StringLength(15)]
        public string Department { get; set; }
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        public List<string> SkillSets { get; set; }
        public bool IsFileContainerExist { get; set; }
        public List<IFormFile> profileFile { get; set; }

        [Display(Name = "Date Of Birth")]
        public string DOB { get; set; }
        [Display(Name = "Date Of Joining")]
        public string DOJ { get; set; }
        public bool IsActive { get; set; }
    }

    public class Pager
    {
        public int pageSize { get; set; } // record per page count
        public int pageCount { get; set; }
        public int currentPage { get; set; }
    }

    public class SessionStatus
    {
        public string token { get; set; }
        public UserRoleModel userrole { get; set; }
        public bool IsValidUser { get; set; }
        public bool SessionSet { get; set; }
        public string Emsg { get; set; }
    }

    public class StudentProfile
    {
        public List<IFormFile> profileFile { get; set; }
    }
    
    public class UrlBase
    {
        public string Baseurl { get; set; }
        public string Baseurl1 { get; set; }
    }
    public class ApiKey
    {
        public string ClientKeyHeader { get; set; }
        public string ClientKey { get; set; }

    }
}
