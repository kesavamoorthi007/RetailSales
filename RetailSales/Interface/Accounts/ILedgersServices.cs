using RetailSales.Models.Accounts;
using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface ILedgersServices
    {
        DataTable GetLedgers();

        DataTable GetEditLedgersDetail(string id);

        DataTable GetAllLedgersGRID(string strStatus);

        DataTable GetAccountName();

        string StatusChange(string tag, string id);

        string RemoveChange(string tag, string id);

        string LedgersCRUD(Ledgers ic);

        

        //string LedgersCRUD(Models.Accounts.Ledgers ic);

    }
}
