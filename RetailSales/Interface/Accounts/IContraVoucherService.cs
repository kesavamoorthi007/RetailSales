using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface IContraVoucherService
    {
        DataTable GetAcc();
        DataTable GetLedgerDetails(string itemId);
    }
}
