﻿using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class RateService : IRateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
       
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEM.ID,VARIANT,UOM,RATE FROM ITEM  Where ITEM.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllRate(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT RATE_BASIC.ID,RATE_BASIC.DOC_NO,FORMAT(RATE_BASIC.DOC_DATE, 'dd-MM-yyyy') AS DOC_DATE,FORMAT(RATE_BASIC.VALID_FROM, 'dd-MM-yyyy') AS VALID_FROM,FORMAT(RATE_BASIC.VALID_TO, 'dd-MM-yyyy') AS VALID_TO,RATE_BASIC.IS_ACTIVE FROM RATE_BASIC WHERE RATE_BASIC.IS_ACTIVE = 'Y' ORDER BY RATE_BASIC.ID DESC";
            }
            else
            {
                SvSql = "SELECT RATE_BASIC.ID,RATE_BASIC.DOC_NO,FORMAT(RATE_BASIC.DOC_DATE, 'dd-MM-yyyy') AS DOC_DATE,FORMAT(RATE_BASIC.VALID_FROM, 'dd-MM-yyyy') AS VALID_FROM,FORMAT(RATE_BASIC.VALID_TO, 'dd-MM-yyyy') AS VALID_TO,RATE_BASIC.IS_ACTIVE FROM RATE_BASIC WHERE RATE_BASIC.IS_ACTIVE = 'N' ORDER BY RATE_BASIC.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        //another
        public DataTable GetEditRate(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,DOC_NO,DOC_DATE,VALID_FROM,VALID_TO FROM RATE_BASIC WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,PRODUCT_NAME FROM ITEM";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVariant(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,PRODUCT_VARIANT FROM PRO_DETAIL WHERE PRO_DETAIL.PRODUCT_CATEGORY='" + id + "'";
           // SvSql = "SELECT ITEM.ID,VARIANT FROM ITEM WHERE ITEM.PRODUCT_DETAIL='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditRateDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,ITEM_NAME,VARIANT,UNIT,RATE FROM RATE_DETAIL WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public string RateCRUD(Rate cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("RatePro", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = cy.ID;
                    }
                    objCmd.Parameters.Add("@docno", SqlDbType.NVarChar).Value = cy.DocNo;
                    objCmd.Parameters.AddWithValue("@docdate", cy.DocDate);
                    objCmd.Parameters.AddWithValue("@validfrom", cy.ValidFrom);
                    objCmd.Parameters.AddWithValue("@validto", cy.ValidTo);
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.RateList != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (RateList  cp in cy.RateList)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into RATE_DETAIL (RATEBASIC_ID,ITEM_NAME,VARIANT,UNIT,RATE) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Rate1 + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete RATE_DETAIL WHERE RATEBASIC_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (RateList cp in cy.RateList)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into RATE_DETAIL (RATEBASIC_ID,ITEM_NAME,VARIANT,UNIT,RATE) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Rate1 + "')";
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

       

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE RATE_BASIC SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE RATE_BASIC SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
