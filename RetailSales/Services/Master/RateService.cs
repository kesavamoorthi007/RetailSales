using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class RateService : IRateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,PRODUCT_NAME FROM ITEM";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEM.ID,VARIANT,UOM,RATE FROM ITEM  Where ITEM.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
