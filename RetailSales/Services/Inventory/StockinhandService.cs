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
        public DataTable GetAllListStockinhand(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT STOCK_IN_HAND.ID,STOCK_IN_HAND.PRODUCT_NAME,STOCK_IN_HAND.VARIANT,STOCK_IN_HAND.BALANCE_QUANTITY,STOCK_IN_HAND.UOM,STOCK_IN_HAND.UNIT_COST,STOCK_IN_HAND.TOTAL_VALUE,STOCK_IN_HAND.IS_ACTIVE FROM STOCK_IN_HAND WHERE STOCK_IN_HAND.IS_ACTIVE = 'Y' ORDER BY STOCK_IN_HAND.ID DESC";
            }
            else
            {
                SvSql = "SELECT STOCK_IN_HAND.ID,STOCK_IN_HAND.PRODUCT_NAME,STOCK_IN_HAND.VARIANT,STOCK_IN_HAND.BALANCE_QUANTITY,STOCK_IN_HAND.UOM,STOCK_IN_HAND.UNIT_COST,STOCK_IN_HAND.TOTAL_VALUE,STOCK_IN_HAND.IS_ACTIVE FROM STOCK_IN_HAND WHERE STOCK_IN_HAND.IS_ACTIVE = 'N' ORDER BY STOCK_IN_HAND.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string Stockinhand(object iD)
        {
            throw new NotImplementedException();
        }
    }
}
