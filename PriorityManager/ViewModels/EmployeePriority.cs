using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PriorityManager.ViewModels
{
    public class EmployeePriority
    {
        public string SelectedEMPID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string IssueNumber { get; set; }
        public string IssueSubject { get; set; }
        public string DevDueDate { get; set; }
        public string QADueDate { get; set; }
        public string Priority { get; set; }
    }
}