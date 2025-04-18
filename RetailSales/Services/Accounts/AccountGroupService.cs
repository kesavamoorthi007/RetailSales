﻿using RetailSales.Interface;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Accounts
{
    public class AccountGroupService : IAccountGroupService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public AccountGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString); 
        }

        // used for Account Class binding and retrieving from database

        public DataTable GetAccountClass()
        {
            string SvSql = string.Empty;
            SvSql = "select ACC_GROUP.ACC_CLASS from ACC_GROUP  WHERE ACC_GROUP.IS_ACTIVE='Y' GROUP BY  ACC_GROUP.ACC_CLASS ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // used for Daydet binding and retrieving from database
        public DataTable GetDaydet()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT VOUCH_NO,FORMAT(T1.VOUCH_DATE, 'dd-MM-yyyy') AS VOUCH_DATE, T1.ID, VOUCH_MEMO, TRANS_TYPE, VOUCH_NAME AS MID, DEBIT_AMT AS DBAMOUNT, CREDIT_AMT AS CRAMOUNT, CASE WHEN TRANS_TYPE = 'Credit' THEN CR_LDGR_NAME ELSE DR_LDGR_NAME END AS ledger, REF_TYPE FROM ACC_VOUCHER T1 WHERE T1.TRANS_STATUS='OPEN'  ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // used for Account Type binding and retrieving from database

        public DataTable GetAccountType()
        {
            string SvSql = string.Empty;
            SvSql = "select ID,ACC_TYPE_NAME from ACC_TYPE where IS_ACTIVE='Y' AND COMP_CODE='028' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // edit and store to database

        public DataTable GetEditAccountGroupDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,ACC_CLASS,ACC_TYPE_CODE,ACC_GRP_NAME FROM ACC_GROUP WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllAccountGroupGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ACC_GROUP.ID,ACC_GROUP.ACC_CLASS,ACC_TYPE.ACC_TYPE_NAME,ACC_GROUP.ACC_GRP_NAME,ACC_GROUP.IS_ACTIVE FROM ACC_GROUP LEFT OUTER JOIN ACC_TYPE ON ACC_TYPE.ID=ACC_GROUP.ACC_TYPE_CODE WHERE ACC_GROUP.IS_ACTIVE = 'Y' ORDER BY ACC_GROUP.ID DESC";
            }
            else
            {
                SvSql = "SELECT ACC_GROUP.ID,ACC_GROUP.ACC_CLASS,ACC_TYPE.ACC_TYPE_NAME,ACC_GROUP.ACC_GRP_NAME,ACC_GROUP.IS_ACTIVE FROM ACC_GROUP LEFT OUTER JOIN ACC_TYPE ON ACC_TYPE.ID=ACC_GROUP.ACC_TYPE_CODE WHERE ACC_GROUP.IS_ACTIVE = 'N' ORDER BY ACC_GROUP.ID DESC";
            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string AccountGroupCRUD(AccountGroup cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("AccGrpProc", objConn);
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


                    objCmd.Parameters.Add("@accclass", SqlDbType.NVarChar).Value = cy.AccountClass;
                    objCmd.Parameters.Add("@acctype", SqlDbType.NVarChar).Value = cy.AccountType;
                    objCmd.Parameters.Add("@accgrpname", SqlDbType.NVarChar).Value = cy.AccountGroupName;


                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = "CreateBy";
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = "UpdateBy";
                        objCmd.Parameters.Add("@updatedon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();

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
                    svSQL = "UPDATE ACC_GROUP SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE ACC_GROUP SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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

        public DataTable GetDaydet(string strfrom, string strTo, string strStatus)
        {
            string SvSql = string.Empty;
            //SvSql = "SELECT VOUCH_NO,FORMAT(T1.VOUCH_DATE, 'dd-MM-yyyy') AS VOUCH_DATE, T1.ID, VOUCH_MEMO, TRANS_TYPE, VOUCH_NAME , MID, DEBIT_AMT, CREDIT_AMT,TRANS_TYPE = 'Credit',THEN CR_LDGR_NAME ELSE DR_LDGR_NAME END AS ledger, REF_TYPE FROM ACC_VOUCHER T1  ";
            SvSql = "SELECT VOUCH_NO,FORMAT(T1.VOUCH_DATE, 'dd-MM-yyyy') AS VOUCH_DATE, T1.ID, VOUCH_MEMO, TRANS_TYPE, VOUCH_NAME AS MID, DEBIT_AMT AS DBAMOUNT, CREDIT_AMT AS CRAMOUNT, CASE WHEN TRANS_TYPE = 'Credit' THEN CR_LDGR_NAME ELSE DR_LDGR_NAME END AS ledger, REF_TYPE FROM ACC_VOUCHER T1 WHERE ID BETWEEN  1 AND  (SELECT MAX(ID)  FROM ACC_VOUCHER)  ";
            //WHERE T1.TRANS_STATUS='OPEN'
            //SvSql = "select VOUCH_NO,FORMAT(T1.VOUCH_DATE, 'dd-MM-yyyy') AS VOUCH_DATE,T1.ID,VOUCH_MEMO,TRANS_TYPE,VOUCH_NAME, MID,DEBIT_AMT,CREDIT_AMT from ACC_VOUCHER T1 ";
            if (!string.IsNullOrEmpty(strfrom) && !string.IsNullOrEmpty(strTo))
            {
                if (strStatus == "Y" || strStatus == null)
                {
                    SvSql += "and T1.IS_ACTIVE ='Y' and T1.VOUCH_DATE BETWEEN '" + strfrom + "' and '" + strTo + "'";
                }
                else
                {
                    SvSql += "and T1.IS_ACTIVE ='N' and T1.VOUCH_DATE BETWEEN '" + strfrom + "' and '" + strTo + "'";
                }
            }
            SvSql += " Order by T1.VOUCH_DATE ASC  ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
