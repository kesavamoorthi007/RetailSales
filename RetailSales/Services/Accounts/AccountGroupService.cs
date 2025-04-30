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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountGroupService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
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
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(ACC_GRP_NAME) as cnt FROM ACC_GROUP WHERE ACC_GRP_NAME = LTRIM(RTRIM('" + cy.AccountGroupName + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Account Group Name Already Exist";
                        return msg;
                    }
                }
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
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = userId;
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = userId;
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
            SvSql = "SELECT T2.TRANS2ID, T1.T1VCHNO, FORMAT(T1.T1VCHDT, 'dd-MMM-yyyy') AS T1VCHDT, T1.TRANS1ID, T1.T1TYPE, T1.T1NARR, T2.DBCR, M.LEDGER_NAME AS MID, T2.DBAMOUNT, T2.CRAMOUNT FROM TRANS1 T1 JOIN TRANS2 T2 ON T1.TRANS1ID = T2.TRANS1ID JOIN ACC_LEDGER M ON M.ID = T2.MID   ";
            if (!string.IsNullOrEmpty(strfrom) && !string.IsNullOrEmpty(strTo))
            {
                SvSql += " Where T1.T1VCHDT BETWEEN '" + strfrom + "' and '" + strTo + "'";
               
          }else
                {
                SvSql += " WHERE T1.T1VCHDT > DATEADD(DAY, -30, GETDATE())";
            }
            SvSql += " ORDER BY T2.TRANS2ID  ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
