using RetailSales.Controllers.Purchase;
using RetailSales.Interface;
using RetailSales.Interface.Master;
using RetailSales.Interface.Purchase;

using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services
{
    public class StockTransferService : IStockTransferService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockTransferService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable Item
        {
            get
            {
                string SvSql = string.Empty;
                SvSql = "SELECT ID,PRODUCT_NAME FROM ITEM";
                DataTable dtt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(dtt);
                return dtt;
            }
        }

        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEM.ID,VARIANT,BIN_NO,UOM,RATE FROM ITEM  Where ITEM.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetFBin()
        {
            string SvSql = string.Empty;
            SvSql = "select BINID,ID from BINMASTER";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
            
        }

        public DataTable GetFlocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCATION_NAME,ID from LOCATION";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetTBin()
        {
            string SvSql = string.Empty;
            SvSql = "select BINID,ID from BINMASTER";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetTlocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCATION_NAME,ID from LOCATION";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
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
    }
    
    //public DataTable GetFBin()
    //{
    //    string SvSql = string.Empty;
    //    SvSql = "select COUNTRY_NAME,ID from COUNTRY";
    //    DataTable dtt = new DataTable();
    //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
    //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
    //    adapter.Fill(dtt);
    //    return dtt;
    //}
}
