using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface ICreditNoteService
    {

        string CreditNoteCRUD(CreditNote cy);

        DataTable GetEditCreditNote(string id);

        DataTable GetCreditNoteDetailes(string id);

        DataTable GetAcc(string id);

        DataTable GetLedgerDetails(string id);

    }
}
