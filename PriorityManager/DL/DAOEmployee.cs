using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace PriorityManager.DL
{
    public static class DAOEmployee
    {
        public static DataTable FetchData(string commandText, List<OleDbParameter> paramList)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbDataReader reader = null;
            OleDbCommand cmd = new OleDbCommand(commandText, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(paramList.ToArray());
                reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public static void ExecuteDMLCommand(string commandText, List<OleDbParameter> paramList)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(commandText, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(paramList.ToArray());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public static string ExecuteScalar(string commandText, List<OleDbParameter> paramList)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string returnVal = "";
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(commandText, conn);
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(paramList.ToArray());
                returnVal = cmd.ExecuteScalar().ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return returnVal;
        }
    }
}