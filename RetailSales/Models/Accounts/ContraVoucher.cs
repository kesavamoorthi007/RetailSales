using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class ContraVoucher
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
        public string Checkno { get; set; }
        public string Chedate { get; set; }
        public string Bname { get; set; }
        public List<Contra> Contralst { get; set; }
    }
    public class Contra
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
