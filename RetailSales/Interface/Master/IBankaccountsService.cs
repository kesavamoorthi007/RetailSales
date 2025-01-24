using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IBankaccountsService
    {
        DataTable GetAccounttype();
        DataTable GetCity();
        DataTable GetCountry();
        DataTable GetState();
    }
}
