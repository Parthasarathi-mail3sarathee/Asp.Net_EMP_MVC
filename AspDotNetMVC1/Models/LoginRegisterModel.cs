using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
