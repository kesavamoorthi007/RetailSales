using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IBankaccountsService
    {
        string BankaccountsCRUD(Bankaccounts ic);
        DataTable GetAccounttype();
        //DataTable GetAllBankaccountsGRID(string strStatus);
        DataTable GetCity();
        DataTable GetCountry();
        DataTable GetState();
    }
}
