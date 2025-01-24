using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class CreditNote
    {
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
        public List<CreditNoteItem> CreditNotelst { get; set; }
    }
    public class CreditNoteItem
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
