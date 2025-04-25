using RetailSales.Interface.Master;
using RetailSales.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class HSNcodeService : IHSNcodeService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HSNcodeService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }
      
         
        public string HSNcodeCRUD(HSNcode ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                string sv = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(HSCODE) as cnt FROM HSNMAST WHERE HSCODE = LTRIM(RTRIM('" + ss.HCode + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "HSN Code Already Exist";
                        return msg;
                    }
                }

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("HsnProc", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "HSNPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", SqlDbType.NVarChar).Value = ss.ID;
                    }

                    objCmd.Parameters.Add("@hcode", SqlDbType.NVarChar).Value = ss.HCode;
                    objCmd.Parameters.Add("@dec", SqlDbType.NVarChar).Value = ss.Dec;
                    objCmd.Parameters.Add("@per", SqlDbType.NVarChar).Value = ss.Per;


                    if (ss.ID == null)
                    {
                       
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = userId;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedon", SqlDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = userId;
                    }
                   
                    objCmd.Parameters.Add("StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (ss.ID != null)
                        {
                            Pid = ss.ID;
                        }

                        if (ss.hsnlst != null)
                        {
                            if (ss.ID == null)
                            {
                                foreach (HSNItem cp in ss.hsnlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into HSNROW (HSNCODEID,TARIFFID,IS_ACTIVE) VALUES ('" + Pid + "','" + cp.tariff + "','Y')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete HSNROW WHERE HSNCODEID='" + ss.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (HSNItem cp in ss.hsnlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into HSNROW (HSNCODEID,TARIFFID,IS_ACTIVE) VALUES ('" + Pid + "','" + cp.tariff + "','Y')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public DataTable GettariffItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TARIFFID from HSNROW where HSNROW.HSNCODEID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Gettariff()
        {
            string SvSql = string.Empty;
            SvSql = "select TAX_NAME,ID from TAXMASTER";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetHSNcode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select HSNMASTID,HSCODE,HSDESC,GSTP from HSNMAST where HSNMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllhsncode(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select IS_ACTIVE,HSNMASTID,HSCODE,HSDESC,GSTP from HSNMAST WHERE IS_ACTIVE='Y' Order by HSNMASTID DESC  ";
            }
            else
            {
                SvSql = "Select IS_ACTIVE,HSNMASTID,HSCODE,HSDESC,GSTP from HSNMAST WHERE IS_ACTIVE='N' Order by HSNMASTID DESC  ";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

       

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE HSNMAST SET IS_ACTIVE ='N' WHERE HSNMASTID='" + id + "'";
                    SqlCommand objCmds = new SqlCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        } 
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE HSNMAST SET IS_ACTIVE ='Y' WHERE HSNMASTID='" + id + "'";
                    SqlCommand objCmds = new SqlCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }
    }

}