using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Accounts
{
    public class Ledgers
    {
        public Ledgers()
        {
            this.AccountGroupList = new List<SelectListItem>();
        }
        public List<SelectListItem> AccountGroupList;

        public string? ID { get; set; }

        public string? AccountGroup { get; set; }

        public string? LedgerName { get; set; }

        public string? AllowZeroValue { get; set; }

        public string? TotalOpeningBalance { get; set; }

        public string? LedgerNotes { get; set; }        

        public string? ddlStatus { get; set; }

    }
    public class ledgergrid
    {
        public string? id { get; set; }
        public string? accountname { get; set; }
        public string? ledgername { get; set; }
        public string? editrow { get; set; }
        public string? delrow { get; set; }
    }
}
