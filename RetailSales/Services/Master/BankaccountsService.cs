using RetailSales.Controllers.Master;
using RetailSales.Interface.Master;
using RetailSales.Models;

namespace RetailSales.Services.Master
{
    public class BankaccountsService : IBankaccountsService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BankaccountsService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
    }
}