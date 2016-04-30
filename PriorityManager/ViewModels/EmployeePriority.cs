using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PriorityManager.ViewModels
{
    public class EmployeePriority
    {
        public string SelectedEMPID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        [Required(ErrorMessage ="{0} is Required.")]
        [Display(Name="Issue Number")]
        public string IssueNumber { get; set; }
        [Required(ErrorMessage = "Issue Subject is Required.")]
        public string IssueSubject { get; set; }
        public string DevDueDate { get; set; }
        public string QADueDate { get; set; }
        public string Priority { get; set; }
        public List<SelectListItem> lstEmployees { get; set; }
    }
}