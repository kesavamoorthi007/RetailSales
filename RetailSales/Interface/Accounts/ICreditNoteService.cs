using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface ICreditNoteService
    {
        DataTable GetAcc();

        string CreditNoteCRUD(CreditNote cy);
    }
}
