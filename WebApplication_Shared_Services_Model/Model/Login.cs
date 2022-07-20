using System.ComponentModel.DataAnnotations;

namespace WebApplication_Shared_Services.Model
{
    public class Login
    {
        [StringLength(20), Required]
        public string username { get; set; }
        [StringLength(20), Required]
        public string password { get; set; }
    }
}
