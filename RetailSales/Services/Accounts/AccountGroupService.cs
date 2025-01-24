using RetailSales.Interface;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
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
            SvSql = "select ACC_GROUP.ID,ACC_GROUP.ACC_CLASS,ACC_GROUP.ACC_TYPE_CODE,ACC_GROUP.IS_ACTIVE from ACC_GROUP  WHERE ACC_GROUP.IS_ACTIVE='Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
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
            SvSql = "select ACC_GROUP.ID,ACC_GROUP.ACC_GRP_NAME,ACC_GROUP.ACC_TYPE_CODE,ACC_GROUP.IS_ACTIVE from ACC_GROUP  WHERE ACC_GROUP.IS_ACTIVE='Y' ";
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
                SvSql = "SELECT ACC_GROUP.ID,ACC_GROUP.ACC_CLASS,ACC_GROUP.ACC_TYPE_CODE,ACC_GROUP.ACC_GRP_NAME,ACC_GROUP.IS_ACTIVE FROM ACC_GROUP /*LEFT OUTER JOIN COUNTRY ON COUNTRY.ID=CITY.COUNTRY_ID LEFT OUTER JOIN STATE ON STATE.ID=CITY.STATE_ID*/ WHERE ACC_GROUP.IS_ACTIVE = 'Y' ORDER BY ACC_GROUP.ID DESC";
            }
            else
            {
                SvSql = "SELECT ACC_GROUP.ID,ACC_GROUP.ACC_CLASS,ACC_GROUP.ACC_TYPE_CODE,ACC_GROUP.ACC_GRP_NAME,ACC_GROUP.IS_ACTIVE FROM ACC_GROUP /*LEFT OUTER JOIN COUNTRY ON COUNTRY.ID=CITY.COUNTRY_ID LEFT OUTER JOIN STATE ON STATE.ID=CITY.STATE_ID*/ WHERE ACC_GROUP.IS_ACTIVE = 'N' ORDER BY ACC_GROUP.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
