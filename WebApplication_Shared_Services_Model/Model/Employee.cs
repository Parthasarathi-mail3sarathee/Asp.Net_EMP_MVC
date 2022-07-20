using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication_Shared_Services.Model
{
    public class Employee
    {
        [StringLength(20), Required]
        public string Name {get;set;}
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
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public bool IsActive { get; set; }
    }
}
