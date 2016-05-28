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
using System.IO;
using System.Globalization;

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
            string commandText = "SELECT EMPNAME,PID,PRIORITY,ISSUENO,SUBJECT,STATUS,DEVDUEDATE,QADUEDATE FROM EMPLOYEES, PRIORITY";
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
            string commandText = "SELECT EMPLOYEES.EMPID,EMPNAME,PRIORITY,ISSUENO,SUBJECT,STATUS,COMPLETED,DEVDUEDATE,QADUEDATE,ENTEREDBY,ASSIGNEDBY,REASON FROM EMPLOYEES, PRIORITY";
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

        public static void UpdateEmployeesFollowingPriority(string empId, int priority)
        {
            string commandText = "UPDATE PRIORITY SET PRIORITY=PRIORITY-1";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            if (empId != null)
            {
                whereClause += (whereClause == "") ? " WHERE EMPID=@empId" : " AND EMID=@empId";
                paramList.Add(new OleDbParameter("@empId", empId));
            }
            if (priority > 0)
            {
                whereClause += (whereClause == "") ? " WHERE PRIORITY>@priority" : " AND PRIORITY>@priority";
                paramList.Add(new OleDbParameter("@priority", priority));
            }
            commandText += whereClause;
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static void UpdatePriorityDetails(EmployeePriority empPriority)
        {
            string commandText = "UPDATE PRIORITY SET ISSUENO=@issueno, SUBJECT=@subject, STATUS=@status, COMPLETED=@completed, DEVDUEDATE=@devdue, QADUEDATE=@qadue";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            empPriority.IssueNumber = (empPriority.IssueNumber == null) ? "" : empPriority.IssueNumber;
            empPriority.IssueSubject = (empPriority.IssueSubject == null) ? "" : empPriority.IssueSubject;
            empPriority.Status = (empPriority.Status == null) ? "" : empPriority.Status;
            empPriority.Completed = (empPriority.Completed == null) ? "0" : empPriority.Completed;
            empPriority.DevDueDate = (empPriority.DevDueDate == null) ? "" : empPriority.DevDueDate;
            empPriority.QADueDate = (empPriority.QADueDate == null) ? "" : empPriority.QADueDate;
            paramList.Add(new OleDbParameter("@issueno", empPriority.IssueNumber));
            paramList.Add(new OleDbParameter("@subject", empPriority.IssueSubject));
            paramList.Add(new OleDbParameter("@status", empPriority.Status));
            paramList.Add(new OleDbParameter("@completed", empPriority.Completed));
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

        public static void AssignEmployeePriority(string assignedBy, string assignTo, string pid, string status, string reason)
        {
            string commandText = "UPDATE PRIORITY SET EMPID=@empid, STATUS=@status, ASSIGNEDBY=@assignedby, REASON=@reason, PRIORITY=@priority";
            string whereClause = "";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            paramList.Add(new OleDbParameter("@empid", assignTo));
            paramList.Add(new OleDbParameter("@status", status));
            paramList.Add(new OleDbParameter("@assignedby", assignedBy));
            paramList.Add(new OleDbParameter("@reason", reason));
            string strPriority = (GetEmployeeMaxPriority(assignTo));
            int priority = (strPriority == "") ? 1 : (Convert.ToInt32(strPriority)+1);
            paramList.Add(new OleDbParameter("@priority", priority));
            if (pid != null)
            {
                whereClause += (whereClause == "") ? " WHERE PID=@pid" : " AND PID=@pid";
                paramList.Add(new OleDbParameter("@pid", pid));
            }
            commandText += whereClause;
            DAOEmployee.ExecuteDMLCommand(commandText, paramList);
        }

        public static EmployeePriority ViewEmployeePriority(string PID)
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
                empPriority.Status = dtPriority.Rows[0]["STATUS"].ToString();
                empPriority.Completed = dtPriority.Rows[0]["COMPLETED"].ToString();
                empPriority.DevDueDate = dtPriority.Rows[0]["DEVDUEDATE"].ToString().Split(' ')[0];
                empPriority.QADueDate = dtPriority.Rows[0]["QADUEDATE"].ToString().Split(' ')[0];
                empPriority.Priority = dtPriority.Rows[0]["PRIORITY"].ToString();
                empPriority.Reason = dtPriority.Rows[0]["REASON"].ToString();
                empPriority.EnteredBy = GetEmployeeName(dtPriority.Rows[0]["ENTEREDBY"].ToString());
                empPriority.AssignedBy = GetEmployeeName(dtPriority.Rows[0]["ASSIGNEDBY"].ToString());
            }
            return empPriority;
        }

        public static string GetEmployeeName(string empId)
        {
            string empName = "";
            DataTable dtEmpName=null;
            if ((empId != null) && (empId != ""))
            {
                dtEmpName = GetEmployees(empId);
            }
            if ((dtEmpName != null) && (dtEmpName.Rows.Count > 0))
            {
                empName = dtEmpName.Rows[0]["EMPNAME"].ToString();
            }
            return empName;
        }

        /*public static EmployeePriority EditEmployeePriority(string PID)
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
        }*/

        public static void AddPriority(EmployeePriority empPriority)
        {
            string commandText = "INSERT INTO PRIORITY(EMPID,ISSUENO,SUBJECT,STATUS,COMPLETED,DEVDUEDATE,QADUEDATE,PRIORITY,ENTEREDBY,ASSIGNEDBY) VALUES(@empId,@issueno,@subject,@status,@completed,@devdue,@qadue,@priority,@enteredby,@assignedby)";
            List<OleDbParameter> paramList = new List<OleDbParameter>();
            empPriority.Completed = (empPriority.Completed == null) ? "0" : empPriority.Completed;
            empPriority.DevDueDate = (empPriority.DevDueDate == null) ? "" : empPriority.DevDueDate;
            empPriority.QADueDate = (empPriority.QADueDate == null) ? "" : empPriority.QADueDate;
            paramList.Add(new OleDbParameter("@empId", empPriority.EmployeeID));
            paramList.Add(new OleDbParameter("@issueno", empPriority.IssueNumber));
            paramList.Add(new OleDbParameter("@subject", empPriority.IssueSubject));
            paramList.Add(new OleDbParameter("@status", empPriority.Status));
            paramList.Add(new OleDbParameter("@completed", empPriority.Completed));
            paramList.Add(new OleDbParameter("@devdue", empPriority.DevDueDate));
            paramList.Add(new OleDbParameter("@qadue", empPriority.QADueDate));
            paramList.Add(new OleDbParameter("@priority", empPriority.Priority));
            paramList.Add(new OleDbParameter("@enteredby", empPriority.EnteredBy));
            paramList.Add(new OleDbParameter("@assignedby", empPriority.AssignedBy));
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

        public static bool UploadPriorityFile(HttpPostedFileBase file, string empId, ref string message, ref string summary, ref List<UploadSummaryReport> uploadSummaryReport)
        {
            uploadSummaryReport = new List<UploadSummaryReport>();
            bool bUpload = true;
            string missingFieldMessage = "Required Field(s) Missing : ";
            string invalidFieldMessage = "Invalid Input(s) : ";
            string rowmessage = "";
            string missingFieldList = "";
            string invalidFieldList = "";
            bool bValidRow = true;
            int recordRow = 0;
            try
            {
                using (StreamReader reader = new StreamReader(file.InputStream))
                {
                    while (true)
                    {
                        missingFieldList = "";
                        invalidFieldList = "";
                        rowmessage = "";
                        bValidRow = true;
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        else if (line.Trim() == "")
                        {
                            continue;
                        }
                        else
                        {
                            UploadSummaryReport report = new UploadSummaryReport();
                            recordRow++;
                            string[] row = line.Trim().Split(',');
                            EmployeePriority empPriority = new EmployeePriority();
                            if ((row.Length >= 1) && (row[0].ToString() != ""))
                            {
                                empPriority.EmployeeID = row[0].ToString();
                                if (GetEmployeeName(empPriority.EmployeeID)=="")
                                {
                                    bValidRow = false;
                                    invalidFieldList += (invalidFieldList != "")? ", " : "";
                                    invalidFieldList += "Employee ID";
                                }                                
                            }
                            else
                            {
                                //required
                                bValidRow = false;
                                missingFieldList += (missingFieldList != "") ? ", " : "";
                                missingFieldList += "Employee ID";
                            }
                            if ((row.Length >= 2) && (row[1].ToString() != ""))
                            {
                                empPriority.IssueNumber = row[1].ToString();
                            }
                            else
                            {
                                //required
                                bValidRow = false;
                                missingFieldList += (missingFieldList != "") ? ", " : "";
                                missingFieldList += "Issue Number";
                            }
                            if ((row.Length >= 3) && (row[2].ToString() != ""))
                            {
                                empPriority.IssueSubject = row[2].ToString();
                            }
                            else
                            {
                                //required
                                bValidRow = false;
                                missingFieldList += (missingFieldList != "") ? ", " : "";
                                missingFieldList += "Issue Subject";
                            }
                            if ((row.Length >= 4) && (row[3].ToString() != ""))
                            {
                                empPriority.Status = row[3].ToString();
                            }
                            else
                            {
                                //required
                                bValidRow = false;
                                missingFieldList += (missingFieldList != "") ? ", " : "";
                                missingFieldList += "Issue Status";
                            }
                            if ((row.Length >= 5) && (row[4] != ""))
                            {
                                empPriority.Completed = row[4].ToString();
                                int completed;
                                if (Int32.TryParse(empPriority.Completed, out completed))
                                {
                                    if ((completed < 0) || (completed >100))
                                    {
                                        //invalid
                                        bValidRow = false;
                                        invalidFieldList += (invalidFieldList != "") ? ", " : "";
                                        invalidFieldList += "% Completed(must be between 0 and 100)";
                                    }
                                }
                                else
                                {
                                    //invalid
                                    bValidRow = false;
                                    invalidFieldList += (invalidFieldList != "") ? ", " : "";
                                    invalidFieldList += "% Completed(must be an integer)";
                                }
                            }
                            else
                            {
                                empPriority.Completed = "0";
                            }
                            if (row.Length >= 6)
                            {
                                empPriority.DevDueDate = row[5].ToString();
                                DateTime devdue;
                                string format = "MM/dd/yyyy";
                                if (!DateTime.TryParseExact(empPriority.DevDueDate, format, CultureInfo.InvariantCulture,DateTimeStyles.None, out devdue))
                                {
                                    //invalid
                                    bValidRow = false;
                                    invalidFieldList += (invalidFieldList != "") ? ", " : "";
                                    invalidFieldList += "Dev Due Date(must be in MM/DD/YYYY format)";
                                }
                            }
                            else
                            {
                                empPriority.DevDueDate = "";
                            }
                            if (row.Length >= 7)
                            {
                                empPriority.QADueDate = row[6].ToString();
                                DateTime qadue;
                                string format = "MM/dd/yyyy";
                                if (!DateTime.TryParseExact(empPriority.QADueDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out qadue))
                                {
                                    //invalid
                                    bValidRow = false;
                                    invalidFieldList += (invalidFieldList != "") ? ", " : "";
                                    invalidFieldList += "QA Due Date(must be in MM/DD/YYYY format)";
                                }
                            }
                            else
                            {
                                empPriority.QADueDate = "";
                            }
                            if (bValidRow)
                            {
                                string maxPriority = GetEmployeeMaxPriority(empPriority.EmployeeID);
                                empPriority.Priority = ((maxPriority == "") ? 1 : Convert.ToInt32(maxPriority) + 1).ToString();
                                empPriority.EnteredBy = empId;
                                empPriority.AssignedBy = empId;
                                AddPriority(empPriority);
                                rowmessage = "Inserted Successfully.";
                            }
                            else
                            {
                                int errorCount = 0;
                                if (missingFieldList != "")
                                {
                                    errorCount++;
                                    rowmessage += (rowmessage == "") ? "Error(s) : " : " ";
                                    rowmessage += errorCount + ". " + missingFieldMessage + missingFieldList;
                                }
                                if (invalidFieldList != "")
                                {
                                    errorCount++;
                                    rowmessage += (rowmessage == "") ? "Error(s) : " : " ";
                                    rowmessage += errorCount + ". " + invalidFieldMessage + invalidFieldList;
                                }
                            }
                            report.RecordNo = recordRow;
                            report.Message = rowmessage;
                            report.Status = (bValidRow == true) ? "PASS" : "FAIL";
                            uploadSummaryReport.Add(report);
                            //rowmessage = "Record " + recordRow + ". " + rowmessage;
                            //summary += (summary != "") ? ("<br/>" + rowmessage) : rowmessage;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                bUpload = false;
                uploadSummaryReport = null;
                message = e.Message;
            }
            return bUpload;
        }

        public static Employee GetEmployeeProfile(string empId)
        {
            Employee emp = null;
            DataTable dtEmployee = GetEmployees(empId);
            if (dtEmployee.Rows.Count > 0)
            {
                emp = new Employee();
                emp.EmpID = dtEmployee.Rows[0]["EMPID"].ToString();
                emp.EmployeeName = dtEmployee.Rows[0]["EMPNAME"].ToString();
                emp.EmailId = dtEmployee.Rows[0]["EMAILID"].ToString();
                emp.UserName = dtEmployee.Rows[0]["USERNAME"].ToString();
                emp.IsManager = dtEmployee.Rows[0]["ISMANAGER"].ToString();
            }
            return emp;
        }
    }
}