using Microsoft.AspNetCore.Mvc.Rendering;
namespace RetailSales.Models
{
    public class DebitNote
    {
        
        public string ID { get; set; }
        public string VocNo { get; set; }
        public string VocDate { get; set; }
        public string ExcRate { get; set; }
        public string RefNo { get; set; }      
        public string RefDate { get; set; }
        public string Currency { get; set; }
        public string Totdeb { get; set; }
        public string Totcri { get; set; }
        public string AmtWd { get; set; }
        public string Narr { get; set; }
        //public string Bname { get; set; }
        public string Createdby { get; set; }
        public string Createdon { get; set; }
        public string Updatedby { get; set; }
        public string Updatedon { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public List<DebitNoteItem> DebitNotelst { get; set; }
    }
    public class DebitNoteItem
    {
        public List<SelectListItem> DBCRlst { get; set; }
        public string DBCR { get; set; }
        public List<SelectListItem> AccNamelst { get; set; }
        public string AccName { get; set; }
        public double DebitAmt { get; set; }
        public double CreditAmt { get; set; }
        public string Balance { get; set; }
        public string Isvalid { get; set; }
    }
}
