using RetailSales.Controllers.Master;
using RetailSales.Interface.Master;
using RetailSales.Models;

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
    }
}