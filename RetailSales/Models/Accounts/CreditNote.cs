using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class CreditNote
    {
        public string ID { get; set; }
        public string VocNo { get; set; }
        public string VocDate { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string exchangeRate { get; set; }
        public string RefNo { get; set; }
        public string ExcRate { get; set; }
        public string RefDate { get; set; }
        public string Currency { get; set; }
        public string totdeb { get; set; }
        public string totcri { get; set; }
        public string totamt { get; set; }
        public string AmtWd { get; set; }
        public string Narr { get; set; }
        public List<CreditNoteItem> CreditNotelst { get; set; }
     
    }
    public class CreditNoteItem
    {
        public string ID { get;set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> DBCRlst { get; set; }
        public string DBCR { get; set; }
        public List<SelectListItem> AccNamelst { get; set; }
        public List<SelectListItem> AccLedgerBalance { get; set; }
        public string AccName { get; set; }
        public double DebitAmt { get; set; }
        public double CreditAmt { get; set; }
        public string Balance { get; set; }
    }

    public class SalesCreditNote
    {
        public string ID { get; set; }
        public string VocNo { get; set; }
        public string VocDate { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string totdeb { get; set; }
        public string totcri { get; set; }
        public string totamt { get; set; }
        public string AmtWd { get; set; }
        public string Narr { get; set; }
        public List<SalesCreditNoteItem> SalesCreditNotelst { get; set; }
    }

    public class SalesCreditNoteItem
    {
        public string ID { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> DBCRlst { get; set; }
        public string DBCR { get; set; }
        public List<SelectListItem> AccNamelst { get; set; }
        public List<SelectListItem> AccLedgerBalance { get; set; }
        public string AccName { get; set; }
        public double DebitAmt { get; set; }
        public double CreditAmt { get; set; }
        public string Balance { get; set; }
    }
}
