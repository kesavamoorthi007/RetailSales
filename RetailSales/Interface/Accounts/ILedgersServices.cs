using RetailSales.Models.Accounts;
using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface ILedgersServices
    {
        //DataTable GetLedgers();
        DataTable GetAccountName();

        DataTable GetEditLedgersDetail(string id);

        string LedgersCRUD(Ledgers cy);

        DataTable GetAllLedgersGRID(string strStatus);       

        string StatusChange(string tag, string id);

        string RemoveChange(string tag, string id);

        //string LedgersCRUD(Models.Accounts.Ledgers ic);

    }
}
