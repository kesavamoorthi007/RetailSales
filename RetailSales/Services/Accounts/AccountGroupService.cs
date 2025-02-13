using RetailSales.Interface;
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
            SvSql = "SELECT VOUCH_NO,  FORMAT(T1.VOUCH_DATE, 'dd-MM-yyyy') AS VOUCH_DATE, T1.ID, VOUCH_MEMO, TRANS_TYPE, VOUCH_NAME AS MID, DEBIT_AMT AS DBAMOUNT, CREDIT_AMT AS CRAMOUNT, CASE WHEN TRANS_TYPE = 'Credit' THEN CR_LDGR_NAME ELSE DR_LDGR_NAME END AS ledger, REF_TYPE FROM ACC_VOUCHER T1 WHERE T1.TRANS_STATUS='OPEN'  ";
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
            SvSql = "select ACC_TYPE_CODE,ACC_TYPE_NAME from ACC_TYPE where IS_ACTIVE='Y' AND COMP_CODE='028' ";
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
                SvSql = "SELECT G.ID, G.ACC_CLASS, G.ACC_TYPE_CODE, G.ACC_GRP_NAME, G.IS_ACTIVE,T.ACC_TYPE_NAME FROM ACC_GROUP G LEFT OUTER JOIN ACC_TYPE T ON G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE G.IS_ACTIVE = 'Y' ORDER BY G.ID DESC";

                //SvSql = "SELECT  G.ID, G.ACC_CLASS, G.ACC_TYPE_CODE, G.ACC_GRP_NAME, G.IS_ACTIVE,T.ACC_TYPE_NAME FROM ACC_GROUP G LEFT OUTER JOIN ACC_TYPE T ON G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE G.IS_ACTIVE = 'Y' AND G.COMP_CODE='028' AND T.COMP_CODE='028' ORDER BY G.ID DESC";
            }
            else
            {
                SvSql = "SELECT G.ID, G.ACC_CLASS, G.ACC_TYPE_CODE, G.ACC_GRP_NAME, G.IS_ACTIVE,T.ACC_TYPE_NAME FROM ACC_GROUP G LEFT OUTER JOIN ACC_TYPE T ON G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE G.IS_ACTIVE = 'N' ORDER BY G.ID DESC";
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

    }
}
