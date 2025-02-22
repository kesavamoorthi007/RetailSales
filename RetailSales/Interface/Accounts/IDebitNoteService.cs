using System.Data;
using RetailSales.Models;

namespace RetailSales.Interface.Accounts
{
    public interface IDebitNoteService
    {
        string DebitNoteCRUD(DebitNote cy);

        DataTable GetAcc(string id);

        DataTable GetLedgerDetails(string id);
    }
}
