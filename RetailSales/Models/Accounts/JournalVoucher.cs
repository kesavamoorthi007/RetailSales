using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class JournalVoucher
    {
        public JournalVoucher()
        {
           
            this.SecIDLst = new List<SelectListItem>();
           

        }
        public string SecID { get; set; }
        public List<SelectListItem> SecIDLst;
        public string VocNo { get; set; }
        public string ID { get; set; }
        public string VocDate { get; set; }
        public string RefNo { get; set; }
        public string ExcRate { get; set; }
        public string RefDate { get; set; }
        public string Currency { get; set; }
        public string totdeb { get; set; }
        public string totcri { get; set; }
        public string AmtWd { get; set; }
        public string Narr { get; set; }
        public string Bname { get; set; }
        public string SSP { get; set; }
        public string Expensive { get; set; }
        public string ICredit { get; set; }
        public string Per { get; set; }
        public string Amt { get; set; }
        public string DueDate { get; set; }
        public List<JournalVoucherItem> JournalVoucherlst { get; set; }
    }
    public class JournalVoucherItem
    {
        public string Isvalid { get; set; }
        public List<SelectListItem> DBCRlst { get; set; }
        public string DBCR { get; set; }
        public List<SelectListItem> AccNamelst { get; set; }
        public string AccName { get; set; }
        public double DebitAmt { get; set; }
        public double CreditAmt { get; set; }
        public string Balance { get; set; }
    }
}
