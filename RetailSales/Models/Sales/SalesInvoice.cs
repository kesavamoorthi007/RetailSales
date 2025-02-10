using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class SalesInvoice
    {
        public string ID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Remarks { get; set; }
        public string Total { get; set; }
        public string TotDis { get; set; }
        public string total { get; set; }
        public string Tot { get; set; }
        public string Return { get; set; }
       
        public string ddlStatus { get; set; }
        public List<SalesInvoiceItem> SalesInvoiceLst { get; set; }
    }
    public class SalesInvoiceItem
    {
        public string Isvalid { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string saveItemId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Bin { get; set; }
        public string Qty { get; set; }
        public string Discount { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Total { get; set; }
        public string ID { get; set; }
        public string ReturnQty { get; set; }


    }
    public class SalesInvoicegrid
    {
        public string id { get; set; }
        public string invno { get; set; }
        public string invdate { get; set; }
        public string customer { get; set; }
        public string address { get; set; }
        public string totalamount { get; set; }
        public string editrow { get; set; }
        public string move { get; set; }
        public string delrow { get; set; }
        public string report { get; set; }

    }

    public class ExinvBasicItem
    {
        public string INVOICE_NO { get; set; }
        public string INV_DATE { get; set; }
        public string CUSTOMER { get; set; }
        public string ADDRESS { get; set; }
      
        public string STATE { get; set; }
        public string GSTNO { get; set; }
      
        public double TOTAL { get; set; }
        public double TOTAL_AMOUNT { get; set; }
        public double DISCOUNT { get; set; }
        
    }
    public class ExinvDetailitem
    {
        public string ITEM { get; set; }
        public string VARIENT { get; set; }
        public string UOM { get; set; }
       
        public double DISCOUNT { get; set; }
        public double QTY { get; set; }
        public double RATE { get; set; }
        public double AMOUNT { get; set; }

    }
}
