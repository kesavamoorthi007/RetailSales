using RetailSales.Interface.Accounts;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Accounts
{
    public class JournalVoucherService : IJournalVoucherService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public JournalVoucherService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAcc()
        {
            string SvSql = string.Empty;
            SvSql = "Select LEDGER_NAME,ID FROM ACC_LEDGER  ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
