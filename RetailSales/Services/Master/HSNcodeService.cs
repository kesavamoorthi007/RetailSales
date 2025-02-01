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
        public HSNcodeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
      
         
        public string HSNcodeCRUD(HSNcode ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                string sv = "";
                //if (ss.ID == null)
                //{

                //    svSQL = " SELECT Count(HSNCODE) as cnt FROM HSNCODE WHERE HSNCODE = LTRIM(RTRIM('" + ss.HCode + "'))";
                //    if (datatrans.GetDataId(svSQL) > 0)
                //    {
                //        msg = "HsnCode Already Existed";
                //        return msg;
                //    }
                //}

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("HSNPROC", objConn);
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

                    objCmd.Parameters.Add("HSCODE", SqlDbType.NVarChar).Value = ss.HCode;
                    objCmd.Parameters.Add("HSDESC", SqlDbType.NVarChar).Value = ss.Dec;
                    objCmd.Parameters.Add("GSTP", SqlDbType.NVarChar).Value = ss.Per;


                    if (ss.ID == null)
                    {
                       
                        objCmd.Parameters.Add("CREATED_ON", SqlDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("CREATED_BY", SqlDbType.NVarChar).Value = ss.createby;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_ON", SqlDbType.Date).Value = DateTime.Now;
                        objCmd.Parameters.Add("UPDATED_BY", SqlDbType.NVarChar).Value = ss.createby;
                    }
                   
                    objCmd.Parameters.Add("StatementType", SqlDbType.NVarChar).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        if (ss.hsnlst != null)
                        {
                            if (ss.ID != null)
                            {
                                Pid = ss.ID;

                                sv = "DELETE HSNROW WHERE HSNCODEID = '" + Pid + "' ";
                                SqlCommand objCmdd = new SqlCommand(sv, objConn);
                                objCmdd.ExecuteNonQuery();
                            }
                            foreach (HSNItem cp in ss.hsnlst)
                            {
                                if (cp.Isvalid == "Y" && cp.tariff != "0")
                                {
                                    using (SqlConnection objConns = new SqlConnection(_connectionString))
                                    {
                                        SqlCommand objCmds = new SqlCommand("HSNROWPROC", objConns);

                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", SqlDbType.NVarChar).Value = DBNull.Value;

                                        objCmds.CommandType = CommandType.StoredProcedure;
                                        objCmds.Parameters.Add("HSNCODEID", SqlDbType.NVarChar).Value = Pid;
                                        objCmds.Parameters.Add("TARIFFID", SqlDbType.NVarChar).Value = cp.tariff;
                                        objCmds.Parameters.Add("IS_ACTIVE", SqlDbType.NVarChar).Value = 'Y';


                                        objCmds.Parameters.Add("StatementType", SqlDbType.NVarChar).Value = StatementType;
                                        objConns.Open();
                                        objCmds.ExecuteNonQuery();
                                        objConns.Close();
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
            SvSql = "select TARIFFID,TARIFFMASTERID from TARIFFMASTER";
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

        //public DataTable GetAllhsncode()
        //{
        //    string SvSql = string.Empty;


        //    SvSql = "Select HSNCODEID,HSNCODE,DESCRIPTION from HSNCODE WHERE ISACTIVE='Y' Order by HSNCODEID DESC  ";

        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable Gethsnitem(string PRID, string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select TARIFFMASTER.TARIFFID,HSNROW.HSNCODEID from HSNROW  LEFT OUTER JOIN TARIFFMASTER ON TARIFFMASTER.TARIFFMASTERID = HSNROW.TARIFFID   Order by HSNCODEID DESC ";
            }
            else
            {
                SvSql = "select TARIFFMASTER.TARIFFID,HSNROW.HSNCODEID from HSNROW  LEFT OUTER JOIN TARIFFMASTER ON TARIFFMASTER.TARIFFMASTERID = HSNROW.TARIFFID   Order by HSNCODEID DESC ";

            } 

            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //public DataTable Gethsnitem(string PRID)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select TARIFFMASTER.TARIFFID,HSNROW.HSNCODEID from HSNROW  LEFT OUTER JOIN TARIFFMASTER ON TARIFFMASTER.TARIFFMASTERID = HSNROW.TARIFFID WHERE HSNROW.IS_ACTIVE ='Y' Order by HSNCODEID DESC ";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        //public DataTable GetCGst()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select * from TAXMAST where TAX='CGST' AND STATUS= 'ACTIVE'  ";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public DataTable GetSGst()
        //{
        //    string SvSql = string.Empty; n
        //    SvSql = "select * from TAXMAST where TAX='SGST' AND STATUS= 'ACTIVE'  ";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public DataTable GetIGst()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select * from TAXMAST where TAX='IGST' AND STATUS= 'ACTIVE'  ";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

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