using RetailSales.Interface.Master;
using RetailSales.Models;

namespace RetailSales.Services.Master
{
    public class CustomerService : ICustomerService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CustomerService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
    }
}

