using RetailSales.Interface.Master;
using RetailSales.Models;

namespace RetailSales.Services.Master
{
    public class SupplierService : ISupplierService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SupplierService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
    }
}
