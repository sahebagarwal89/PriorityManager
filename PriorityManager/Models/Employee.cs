using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriorityManager.Models
{
    public class Employee
    {
        [Display(Name = "Employee ID")]
        public string EmpID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Manager")]
        public string IsManager { get; set; }
        [Display(Name = "Active")]
        public string IsActive { get; set; }
    }
}