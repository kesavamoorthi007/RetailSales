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
            SvSql = "SELECT PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,INVENTORY_ITEM.UOM,SUM(BALANCE_QTY) AS BALANCE_QTY,LOCATION_ID FROM INVENTORY_ITEM LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=INVENTORY_ITEM.ITEM_ID LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=INVENTORY_ITEM.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=INVENTORY_ITEM.VARIANT GROUP BY PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,INVENTORY_ITEM.UOM,LOCATION_ID";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
