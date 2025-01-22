using RetailSales.Controllers.Master;
using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class ProductdetailService : IProductdetailService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductdetailService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select PRODUCT_NAME,ID From PRODUCT";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}