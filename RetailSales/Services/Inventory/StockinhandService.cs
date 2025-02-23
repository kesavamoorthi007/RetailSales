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
                SvSql = "SELECT INVENTORY_ITEM_ID,DOC_ID,ITEM_ID,VARIANT,UOM,BALANCE_QTY,IS_ACTIVE FROM INVENTORY_ITEM WHERE INVENTORY_ITEM.IS_ACTIVE = 'Y' ORDER BY INVENTORY_ITEM.INVENTORY_ITEM_ID DESC";
            }
            else
            {
                SvSql = "SELECT INVENTORY_ITEM_ID,DOC_ID,ITEM_ID,VARIANT,UOM,BALANCE_QTY,IS_ACTIVE FROM INVENTORY_ITEM WHERE INVENTORY_ITEM.IS_ACTIVE = 'N' ORDER BY INVENTORY_ITEM.INVENTORY_ITEM_ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
