using PriorityManager.DL;
using PriorityManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace PriorityManager.BL
{
    public static class ManageAccount
    {
        public static void SaveUser(RegisterModel UserDetails)
        {
            string commandText = "INSERT INTO EMPLOYEES(EMPID,EMPNAME,EMAILID,USERNAME,PASS,ISMANAGER,ISACTIVE) VALUES(@empId, @empName, @emailId, @username, @pass, @IsManager, @IsActive)";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            paramList.Add(new OleDbParameter("@empId", UserDetails.EMPID));
            paramList.Add(new OleDbParameter("@empName", UserDetails.FullName));
            paramList.Add(new OleDbParameter("@emailId", UserDetails.EmailId));
            paramList.Add(new OleDbParameter("@username", UserDetails.UserName));
            paramList.Add(new OleDbParameter("@pass", UserDetails.Password));
            paramList.Add(new OleDbParameter("@IsManager", UserDetails.IsManager));
            paramList.Add(new OleDbParameter("@IsActive", "No"));
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static bool ValidateUser(LoginModel User, ref bool IsActive, ref Employee employee)
        {
            IsActive = true;
            employee = GetEmployee(User.UserName,User.Password);
            bool IsValid = true;
            if (employee == null)
            {
                IsValid = false;
            }
            else
            {
                if(employee.IsActive == "No")
                {
                    IsActive = false;
                    IsValid = false;
                }
            }
            return IsValid;
        }

        //userDetails[0] - USERNAME
        //userDetails[1] - PASSWORD
        //userDetails[2] - EMPID
        //userDetails[3] - EMAILID
        public static Employee GetEmployee(params string[] userDetails)
        {
            Employee employee = null;
            string commandText = "SELECT EMPID, EMPNAME, USERNAME, ISMANAGER, ISACTIVE FROM EMPLOYEES";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if ((userDetails.Length >= 1) && (userDetails[0] != null))
            {
                whereClause += (whereClause == "") ? " WHERE USERNAME=@username" : " AND USERNAME=@username";
                paramList.Add(new OleDbParameter("@username", userDetails[0]));
            }
            if ((userDetails.Length >= 2) && (userDetails[1] != null))
            {
                whereClause += (whereClause == "") ? " WHERE PASS=@pass" : " AND PASS=@pass";
                paramList.Add(new OleDbParameter("@pass", userDetails[1]));
            }
            if ((userDetails.Length >= 3) && (userDetails[2] != null))
            {
                whereClause += (whereClause == "") ? " WHERE EMPID=@empId" : " AND EMPID=@empId";
                paramList.Add(new OleDbParameter("@empId", userDetails[2]));
            }
            if ((userDetails.Length >= 4) && (userDetails[3] != null))
            {
                whereClause += (whereClause == "") ? " WHERE EMAILID=@emailId" : " AND EMAILID=@emailId";
                paramList.Add(new OleDbParameter("@emailId", userDetails[3]));
            }
            commandText += whereClause;
            DataTable dtEmployee = DAOEmployee.FetchData(commandText, paramList);
            if (dtEmployee.Rows.Count > 0)
            {
                employee = new Employee();
                employee.EmpID = dtEmployee.Rows[0]["EMPID"].ToString();
                employee.EmployeeName = dtEmployee.Rows[0]["EMPNAME"].ToString();
                employee.UserName = dtEmployee.Rows[0]["USERNAME"].ToString();
                employee.IsManager = dtEmployee.Rows[0]["ISMANAGER"].ToString();
                employee.IsActive = dtEmployee.Rows[0]["ISACTIVE"].ToString();
            }
            return employee;
        }
    }
}