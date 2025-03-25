using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface IJournalVoucherService
    {
        DataTable GetAcc();
        DataTable GetLedgerDetails(string itemId);
    }
}
