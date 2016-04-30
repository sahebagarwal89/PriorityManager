using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PriorityManager.DL;
using System.Web.Script.Serialization;
using PriorityManager.Models;
using System.Data.OleDb;
using PriorityManager.ViewModels;

namespace PriorityManager.BL
{
    public static class EmployeeDetails
    {
        public static DataTable GetEmployees(string empId)
        {
            string commandText = "SELECT * FROM EMPLOYEES";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (empId != null)
            {
                whereClause += (whereClause == "") ? " WHERE EMPID=@empId" : " AND EMPID=@empId";
                paramList.Add(new OleDbParameter("@empId", empId));
            }
            commandText += whereClause;
            DataTable dtEmployee = DAOEmployee.FetchData(commandText, paramList);
            return dtEmployee;
        }

        public static DataTable GetEmployeePriority(string empId)
        {
            string commandText = "SELECT EMPNAME,PID,PRIORITY,ISSUENO,SUBJECT,DEVDUEDATE,QADUEDATE FROM EMPLOYEES, PRIORITY";
            string whereClause = " WHERE EMPLOYEES.EMPID=PRIORITY.EMPID";
            string orderBy = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (empId == null)
            {
                orderBy = " ORDER BY EMPNAME, PRIORITY";
            }
            if (empId != null)
            {
                whereClause += (whereClause == "") ? " WHERE EMPLOYEES.EMPID=@empId" : " AND EMPLOYEES.EMPID=@empId";
                paramList.Add(new OleDbParameter("@empId", empId));
                orderBy = " ORDER BY PRIORITY";
            }
            commandText += whereClause + orderBy;
            DataTable dtEmpPriority = DAOEmployee.FetchData(commandText, paramList);
            return dtEmpPriority;
        }

        public static DataTable GetEmployeePriorityById(string PID)
        {
            string commandText = "SELECT EMPLOYEES.EMPID,EMPNAME,PRIORITY,ISSUENO,SUBJECT,DEVDUEDATE,QADUEDATE FROM EMPLOYEES, PRIORITY";
            string whereClause = " WHERE EMPLOYEES.EMPID=PRIORITY.EMPID";
            string orderBy = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (PID != null)
            {
                whereClause += (whereClause == "") ? " WHERE PID=@pid" : " AND PID=@pid";
                paramList.Add(new OleDbParameter("@pid", PID));
            }
            commandText += whereClause + orderBy;
            DataTable dtEmpPriority = DAOEmployee.FetchData(commandText, paramList);
            return dtEmpPriority;
        }

        public static void DeleteEmployeePriority(string PID)
        {
            string commandText = "DELETE FROM PRIORITY";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (PID != null)
            {
                whereClause += (whereClause == "") ? " WHERE PID=@pid" : " AND PID=@pid";
                paramList.Add(new OleDbParameter("@pid", PID));
            }
            commandText += whereClause;
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static void SwapEmployeePriority(string empId, string firstPriority, string secondPriority)
        {
            string firstRowId = GetEmployeePriorityRow(empId, firstPriority);
            string secondRowId = GetEmployeePriorityRow(empId, secondPriority);
            UpdateEmployeePriority(firstRowId, secondPriority);
            UpdateEmployeePriority(secondRowId, firstPriority);
        }

        public static string GetEmployeePriorityRow(string empId, string priority)
        {
            string priorityRowId = "";
            string commandText = "SELECT * FROM PRIORITY";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (empId != null)
            {
                whereClause += (whereClause == "") ? " WHERE EMPID=@empId" : " AND EMPID=@empId";
                paramList.Add(new OleDbParameter("@empId",empId));
            }
            if (priority != null)
            {
                whereClause += (whereClause == "") ? " WHERE PRIORITY=@priority" : " AND PRIORITY=@priority";
                paramList.Add(new OleDbParameter("@priority", priority));
            }
            commandText += whereClause;
            DataTable dtPriority = DAOEmployee.FetchData(commandText, paramList);
            priorityRowId = dtPriority.Rows[0]["PID"].ToString();
            return priorityRowId;
        }

        public static void UpdateEmployeePriority(string priorityRowId,string priority)
        {
            string commandText = "UPDATE PRIORITY SET PRIORITY=@priority";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            paramList.Add(new OleDbParameter("@priority", priority));
            if (priorityRowId != null)
            {
                whereClause += (whereClause == "") ? " WHERE PID=@priorityRowId" : " AND PID=@priorityRowId";
                paramList.Add(new OleDbParameter("@priorityRowId", priorityRowId));
            }
            commandText += whereClause;
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static void UpdatePriorityDetails(EmployeePriority empPriority)
        {
            string commandText = "UPDATE PRIORITY SET ISSUENO='" + empPriority.IssueNumber + "', SUBJECT='" + empPriority.IssueSubject + "', DEVDUEDATE='" + empPriority.DevDueDate + "', QADUEDATE='" + empPriority.QADueDate+"'";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            empPriority.IssueNumber = (empPriority.IssueNumber == null) ? "" : empPriority.IssueNumber;
            empPriority.IssueSubject = (empPriority.IssueSubject == null) ? "" : empPriority.IssueSubject;
            empPriority.DevDueDate = (empPriority.DevDueDate == null) ? "" : empPriority.DevDueDate;
            empPriority.QADueDate = (empPriority.QADueDate == null) ? "" : empPriority.QADueDate;
            paramList.Add(new OleDbParameter("@issueno", empPriority.IssueNumber));
            paramList.Add(new OleDbParameter("@subject", empPriority.IssueSubject));
            paramList.Add(new OleDbParameter("@devdue", empPriority.DevDueDate));
            paramList.Add(new OleDbParameter("@qadue", empPriority.QADueDate));
            if (empPriority.EmployeeID != null)
            {
                whereClause += (whereClause == "") ? " WHERE EMPID=@empId" : " AND EMPID=@empId";
                paramList.Add(new OleDbParameter("@empId", empPriority.EmployeeID));
            }
            if (empPriority.Priority != null)
            {
                whereClause += (whereClause == "") ? " WHERE PRIORITY=@priority" : " AND PRIORITY=@priority";
                paramList.Add(new OleDbParameter("@priority", empPriority.Priority));
            }
            commandText += whereClause;
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static EmployeePriority EditEmployeePriority(string PID)
        {
            EmployeePriority empPriority = null;
            DataTable dtPriority = GetEmployeePriorityById(PID);
            if (dtPriority.Rows.Count > 0)
            {
                empPriority = new EmployeePriority();
                empPriority.EmployeeID = dtPriority.Rows[0]["EMPID"].ToString(); ;
                empPriority.EmployeeName = dtPriority.Rows[0]["EMPNAME"].ToString();
                empPriority.IssueNumber = dtPriority.Rows[0]["ISSUENO"].ToString();
                empPriority.IssueSubject = dtPriority.Rows[0]["SUBJECT"].ToString();
                empPriority.DevDueDate = dtPriority.Rows[0]["DEVDUEDATE"].ToString().Split(' ')[0];
                empPriority.QADueDate = dtPriority.Rows[0]["QADUEDATE"].ToString().Split(' ')[0];
                empPriority.Priority = dtPriority.Rows[0]["PRIORITY"].ToString();
            }
            return empPriority;
        }

        public static void AddPriority(EmployeePriority empPriority)
        {
            string commandText = "INSERT INTO PRIORITY(EMPID,ISSUENO,SUBJECT,DEVDUEDATE,QADUEDATE,PRIORITY) VALUES(@empId, @issueno, @subject, @devdue, @qadue, @priority)";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            paramList.Add(new OleDbParameter("@empId", empPriority.EmployeeID));
            paramList.Add(new OleDbParameter("@issueno", empPriority.IssueNumber));
            paramList.Add(new OleDbParameter("@subject", empPriority.IssueSubject));
            paramList.Add(new OleDbParameter("@devdue", empPriority.DevDueDate));
            paramList.Add(new OleDbParameter("@qadue", empPriority.QADueDate));
            paramList.Add(new OleDbParameter("@priority", empPriority.Priority));
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static string GetEmployeeMaxPriority(string empId)
        {
            string commandText = "SELECT MAX(PRIORITY) FROM PRIORITY";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (empId != null)
            {
                whereClause += (whereClause == "") ? " WHERE EMPID=@empId" : " AND EMPID=@empId";
                paramList.Add(new OleDbParameter("@empId", empId));
            }
            commandText += whereClause;
            string maxPriority = DAOEmployee.ExecuteScalar(commandText, paramList);
            return maxPriority;
        }

        public static string DataTableToJsonWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col].ToString());
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
    }
}