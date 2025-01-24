using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Accounts
{
    public class AccountGroup
    {

        public AccountGroup() 
        {
            this.accclasslist = new List<SelectListItem>();
            this.acctypelist = new List<SelectListItem>();
        }
        public List<SelectListItem> accclasslist;
        public List<SelectListItem> acctypelist;

        public string ID { get; set; }
        public string AccountClass { get; set; }
        public string AccountType { get; set; }
        public string AccountGroupName { get; set; }
        public string ddlstatus { get; set; }
        
    }

    public class ListAccountGroupgrid
    {
        public string id { get; set; }
        public string accclass { get; set; }
        public string acctype { get; set; }
        public string accgrpname { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
        
    }

    public class ListDayItems
    {
        public string id { get; set; }
        public string vocherno { get; set; }
        public string vocherdate { get; set; }
        public string tratype { get; set; }
        public string vocmemo { get; set; }
        public string vtype { get; set; }
        public string type { get; set; }
        public string ledgercode { get; set; }
        public string debitamount { get; set; }
        public string creditamount { get; set; }

    }
}
