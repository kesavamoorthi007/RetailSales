using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Accounts
{
    public class PaymentRequest
    {
        public PaymentRequest() 
        {
            this.Suplst = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public List<SelectListItem> Suplst {  get; set; }
        public string SuppName { get; set; }
        public string ReqBy { get; set; }
        public string TotReqAmt { get; set; }
        public string ddlStatus { get; set; }
        public List<GRNItem> GRNlst { get; set; }
        
    }
    public class GRNItem
    {
        public string ID { get; set; }
        public string GRNNo { get; set; }
        public string GRNAmt { get; set; }
        public string ReqAmt { get; set; }
        public string PendAmt { get; set; }
        public string Isvalid { get; set; }
    }
    public class ListPaymentRequestgrid
    {
        public string id { get; set; }
        public string suppname { get; set; }
        public string reqby { get; set; }
        public string totreqamt { get; set; }
        public string view { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
