using RetailSales.Interface;
using RetailSales.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using RetailSales.Services.Purchase;

namespace RetailSales.Services 
{
    public class AccConfigService : IAccConfig
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccConfigService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        

        //public IEnumerable<ConfigItem> GetAllConfigItem(string id)
        //{
        //    List<ConfigItem> cmpList = new List<ConfigItem>();
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {

        //        using (SqlCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "SELECT ADTYPE,ADNAME,ADSCHEMENAME,ADACCOUNT FROM ADCOMPD WHERE ADCOMPD.ACTIVE ='" + id + "'";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ConfigItem cmp = new ConfigItem
        //                {
        //                    Type = rdr["ADTYPE"].ToString(),
        //                    Tname = rdr["ADNAME"].ToString(),
        //                    Schname = rdr["ADSCHEMENAME"].ToString(),
        //                    ledger = rdr["ADACCOUNT"].ToString()
        //            };
                        
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
        public string ConfigCRUD(AccConfig cy)
        {
            string msg = "";
            try
            {

                string StatementType = string.Empty;
                string svSQL = "";
                string sv = "";
                
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(*) as cnt FROM ADCOMPH WHERE ADSCHEME =LTRIM(RTRIM('" + cy.Scheme + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "ETariff Already Existed";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("ACONFIGPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = cy.ID;

                    }
                    
                    objCmd.Parameters.Add("@TName", SqlDbType.NVarChar).Value = cy.TransactionName;
                    objCmd.Parameters.Add("@Tid", SqlDbType.NVarChar).Value = cy.TransactionID;
                    
                    objCmd.Parameters.Add("@Scheme", SqlDbType.NVarChar).Value = cy.Scheme;
                    objCmd.Parameters.Add("@Descrip", SqlDbType.NVarChar).Value = cy.SchemeDes;

                    objCmd.Parameters.Add("@Bid", SqlDbType.Int).Value = "0";
                    objCmd.Parameters.Add("@Crby", SqlDbType.NVarChar).Value = cy.CreatBy;
                    objCmd.Parameters.Add("@Cron", SqlDbType.Date).Value = DateTime.Now;
                    objCmd.Parameters.Add("@Cudate", SqlDbType.Date).Value = DateTime.Now;

                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    //objCmd.Parameters.Add("@OUTID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;

                            sv = "DELETE ADCOMPD WHERE ADCOMPHID = '" + Pid + "' ";
                            SqlCommand objCmdd = new SqlCommand(sv, objConn);
                            objCmdd.ExecuteNonQuery();
                        }
                        foreach (ConfigItem cp in cy.ConfigLst)
                        {
                            if (cp.Isvalid == "Y" && cp.ledger != "0")
                            {
                                using (SqlConnection objConns = new SqlConnection(_connectionString))
                                {
                                    SqlCommand objCmds = new SqlCommand("ADCOMPD_PROC", objConns);
                                   
                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("@ID", SqlDbType.NVarChar).Value = DBNull.Value;
                                   
                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    objCmds.Parameters.Add("@Pid", SqlDbType.NVarChar).Value = Pid;
                                    objCmds.Parameters.Add("@Ttype", SqlDbType.NVarChar).Value = cp.Type;
                                    objCmds.Parameters.Add("@Tname", SqlDbType.NVarChar).Value = cp.Tname;
                                    objCmds.Parameters.Add("@Scheme", SqlDbType.NVarChar).Value = cp.Schname;
                                    objCmds.Parameters.Add("@Acco", SqlDbType.NVarChar).Value = cp.ledger;

                                    objCmds.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                                    objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }

                            }

                        }
                    }

                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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
       

        public DataTable GetAccConfig(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,CURRENT_DATE ,CREATED_BY,CREATED_ON,CURRENT_DATE,BRANCHID,ADCOMPHID FROM ADCOMPH WHERE ADCOMPHID = '" + id + "' ";
            SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID FROM ADCOMPH WHERE ADCOMPHID = '" + id + "' ";
            
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
        public DataTable Getledger()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LEDGER_NAME,ID FROM ACC_LEDGER WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccConfigItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADTYPE,ADNAME,ADSCHEMENAME,ADACCOUNT from ADCOMPD where ADCOMPD.ADCOMPHID = '" + id + "' "; 
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
                    svSQL = "UPDATE ADCOMPH SET IS_ACTIVE ='N' WHERE ADCOMPHID='" + id + "'";
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
                    svSQL = "UPDATE ADCOMPH SET IS_ACTIVE ='Y' WHERE ADCOMPHID = '" + id + "'";
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

        public DataTable GetConfig(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID FROM ADCOMPH WHERE ADCOMPHID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetConfigItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ADTYPE,ADNAME,ADSCHEMENAME,M.LEDGER_NAME LEDNAME from ADCOMPD A LEFT OUTER JOIN ACC_LEDGER M ON M.ID = A.ADACCOUNT where A.ADCOMPHID= '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllConfig(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID,IS_ACTIVE FROM ADCOMPH WHERE IS_ACTIVE = 'Y' ORDER BY ADCOMPHID ASC";

            }
            else
            {
                SvSql = "Select ADSCHEMEDESC,ADSCHEME,ADTRANSDESC,ADTRANSID,ADCOMPHID,IS_ACTIVE FROM ADCOMPH WHERE IS_ACTIVE = 'N' ORDER BY ADCOMPHID ASC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
