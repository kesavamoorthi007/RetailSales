using RetailSales.Controllers;
using RetailSales.Interface;
using RetailSales.Interface;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services
{
    public class StockinhandService : IStockinhandService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockinhandService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllListStockinhand()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT   ITEM_ID,VARIANT,UOM,SUM(BALANCE_QTY) AS BALANCE_QTY FROM INVENTORY_ITEM  GROUP BY ITEM_ID,VARIANT,UOM";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
