namespace RetailSales.Models
{
    public class SalesInvoiceReport
    {
        public string ID { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdd { get; set; }
        public string CustomerPhNo { get; set; }
        public string TotAmt { get; set; }
        public string Discount { get; set; }
        public string RoundOff { get; set; }
        public string PayAmt { get; set; }
        public string AmtInWords { get; set; }
        public List<SalesInvoiceReportItem> SalesInvoiceReportlst { get; set; }
    }
    public class SalesInvoiceReportItem
    {
        public string ID { get; set; }
        public string Item { get; set; }
        public string HSN { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Per { get; set; }
        public string DiscPercent { get; set; }
        public string Total { get; set; }
        public string Isvalid { get; set; }
    }
}
