﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using PriorityManager.BL;
using System.Web.Script.Serialization;
using PriorityManager.Models;
using PriorityManager.ViewModels;

namespace PriorityManager.Controllers
{
    public class EmployeePriorityController : Controller
    {
        //
        // GET: /DisplayPriority/

        public ActionResult DisplayPriority(string empId)
        {
            EmployeePriority empPriority = new EmployeePriority();
            empPriority.lstEmployees = new List<SelectListItem>();
            DataTable dtEmployee = EmployeeDetails.GetEmployees(null);
            bool bIsValidEmployee = false;
            foreach (DataRow drEmp in dtEmployee.Rows)
            {
                empPriority.lstEmployees.Add(new SelectListItem { Text = drEmp["EMPNAME"].ToString(), Value = drEmp["EMPID"].ToString() });
                if ((empId != null) && (empId == drEmp["EMPID"].ToString()))
                {
                    bIsValidEmployee = true;
                }
            }

            if (empPriority.lstEmployees.Count > 0)
            {
                empPriority.lstEmployees.Add(new SelectListItem { Text = "All Employees", Value = "-1" });
            }

            if ((empId == null) || ((empId != null) && (empPriority.lstEmployees.Count > 0) && (empId == "-1")))
            {
                bIsValidEmployee = true;
            }

            if (!bIsValidEmployee)
            {
                return RedirectToAction("ErrorMessage", "Error", new { message = "Employee Id is not valid!" });
            }

            empPriority.SelectedEMPID = empId;
            return View(empPriority);
        }

        public ActionResult GetEmployeePriority(string empId)
        {
            if (empId == "-1")
            {
                empId = null;
            }
            DataTable dtEmpPriority = EmployeeDetails.GetEmployeePriority(empId);
            string employeePriority = EmployeeDetails.DataTableToJsonWithJavaScriptSerializer(dtEmpPriority);
            return Json(employeePriority);
        }

        public ActionResult DeleteEmployeePriority(string empId, string PID)
        {
            EmployeeDetails.DeleteEmployeePriority(PID);
            if (empId == "-1")
            {
                empId = null;
            }
            return GetEmployeePriority(empId);
        }

        public ActionResult SwapEmployeePriority(string empId, string firstPriority, string secondPriority)
        {
            EmployeeDetails.SwapEmployeePriority(empId, firstPriority, secondPriority);
            return GetEmployeePriority(empId);
        }

        public ActionResult ViewPriority(string empId, string PID)
        {
            EmployeePriority empPriority = EmployeeDetails.ViewEmployeePriority(PID);
            if (empPriority == null)
            {
                return HttpNotFound();
            }
            empPriority.SelectedEMPID = empId;
            return View(empPriority);
        }

        public ActionResult EditPriority(string empId, string PID)
        {
            EmployeePriority empPriority = EmployeeDetails.ViewEmployeePriority(PID);
            if (empPriority == null)
            {
                return HttpNotFound();
            }
            empPriority.SelectedEMPID = empId;
            return View(empPriority);
        }

        [HttpPost]
        public ActionResult EditPriority(EmployeePriority empPriority)
        {
            if (ModelState.IsValid)
            {
                EmployeeDetails.UpdatePriorityDetails(empPriority);
                return RedirectToAction("DisplayPriority", new { empId = empPriority.SelectedEMPID});
            }
            return View(empPriority);
        }

        public ActionResult AddPriority(string empId)
        {
            DataTable dtEmployee = EmployeeDetails.GetEmployees(empId);
            if (dtEmployee.Rows.Count == 0)
            {
                return HttpNotFound();
            }
            EmployeePriority empPriority = new EmployeePriority();
            empPriority.EmployeeID = empId;
            empPriority.EmployeeName = dtEmployee.Rows[0]["EMPNAME"].ToString();
            string maxPriority = EmployeeDetails.GetEmployeeMaxPriority(empId);
            empPriority.Priority = ((maxPriority == "") ? 1 : Convert.ToInt32(maxPriority) + 1).ToString();
            return View(empPriority);
        }

        [HttpPost]
        public ActionResult AddPriority(EmployeePriority empPriority)
        {
            if (ModelState.IsValid)
            {
                EmployeeDetails.AddPriority(empPriority);
                return RedirectToAction("DisplayPriority", new { empId = empPriority.EmployeeID });
            }

            return View(empPriority);
        }

        public ActionResult AssignPriority(string empId, string assignedBy, string assignTo, string pid, string status, string reason)
        {
            EmployeePriority empPriority = EmployeeDetails.ViewEmployeePriority(pid);
            EmployeeDetails.AssignEmployeePriority(assignedBy, assignTo, pid, status,reason);
            EmployeeDetails.UpdateEmployeesFollowingPriority(empPriority.EmployeeID, Convert.ToInt32(empPriority.Priority));
            return GetEmployeePriority(empId);
        }
    }
}
