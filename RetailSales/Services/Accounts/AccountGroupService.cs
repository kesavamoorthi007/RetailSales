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
                SvSql = "SELECT  G.ID, G.ACC_CLASS, G.ACC_TYPE_CODE, G.ACC_GRP_NAME, G.IS_ACTIVE,T.ACC_TYPE_NAME FROM ACC_GROUP G LEFT OUTER JOIN ACC_TYPE T ON G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE G.IS_ACTIVE = 'Y' AND G.COMP_CODE='028' AND T.COMP_CODE='028' ORDER BY G.ID DESC";
            }
            else
            {
                SvSql = "SELECT  G.ID, G.ACC_CLASS, G.ACC_TYPE_CODE, G.ACC_GRP_NAME, G.IS_ACTIVE,T.ACC_TYPE_NAME FROM ACC_GROUP G LEFT OUTER JOIN ACC_TYPE T ON G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE G.IS_ACTIVE = 'N' AND G.COMP_CODE='028' AND T.COMP_CODE='028' ORDER BY G.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
