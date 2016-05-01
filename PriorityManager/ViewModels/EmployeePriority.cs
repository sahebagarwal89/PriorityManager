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
        [Display(Name = "Selected EMPID")]
        public string SelectedEMPID { get; set; }
        [Display(Name = "Employee ID")]
        public string EmployeeID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage ="{0} is Required.")]
        [Display(Name="Issue Number")]
        public string IssueNumber { get; set; }
        [Required(ErrorMessage = "{0} is Required.")]
        [Display(Name = "Issue Subject")]
        public string IssueSubject { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Dev Due Data")]
        public string DevDueDate { get; set; }
        [Display(Name = "QA Due Date")]
        public string QADueDate { get; set; }
        [Display(Name = "Priority")]
        public string Priority { get; set; }
        [Display(Name = "Entered By")]
        public string EnteredBy { get; set; }
        [Display(Name = "Assigned By")]
        public string AssignedBy { get; set; }
        [Display(Name = "Reason")]
        public string Reason { get; set; }
        public List<SelectListItem> lstEmployees { get; set; }
    }
}