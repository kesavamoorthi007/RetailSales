using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RetailSales.Models
{
    public class SalesInvoice
    {
        public SalesInvoice()
        {
            
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();
            this.Paymentlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public List<SelectListItem> Paymentlst;
        public string ID { get; set; }
        public string Payment { get; set; }
        public string InvoiceNo { get; set; }
        public string DocNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Remarks { get; set; }
        public string Gross { get; set; }
        public string Round { get; set; }
        public string Disc { get; set; }
        public string Net { get; set; }
        public string Frieghtcharge { get; set; }
        public string Amountinwords { get; set; }
        public string Return { get; set; }
        public string Rt { get; set; }
        public string DiscPre { get; set; }

        public string ddlStatus { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "CVV must be exactly 3 digits")]
        public string cvv { get; set; }
        public string CashRecivedBy { get; set; }
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string Upi { get; set; }
        public string UpiRefNo { get; set; }
        public string BankName { get; set; }
        public string Transaction { get; set; }

        public List<SalesInvoiceItem> SalesInvoiceLst { get; set; }
    }
    public class SalesInvoiceItem
    {
        public string saveItemId { get; set; }
        public string sno { get; set; }
        public List<SelectListItem> Itemlst { get; set; }       
        public string Item { get; set; }
        public List<SelectListItem> Productlst { get; set; }
        public string Product { get; set; }
        public List<SelectListItem> Varientlst { get; set; }
        public string Varient { get; set; }
        public string Hsn { get; set; }
        public string UOM { get; set; }
        public string StockQty { get; set; }
        public string GStockQty { get; set; }
        public string Qty { get; set; }
        public List<SelectListItem> UOMlst { get; set; }
        public string DestUOM { get; set; }
        public string CF { get; set; }
        public string CfQty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string DiscPer { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }
        public string Isvalid { get; set; }             
        public string ID { get; set; }        
        public string Bin { get; set; }
        public string ReturnQty { get; set; }
        
    }
    public class SalesInvoicegrid
    {
        public string id { get; set; }
        public string rt { get; set; }
        public string invno { get; set; }
        public string invdate { get; set; }
        public string customer { get; set; }
        public string address { get; set; }
        public string totalamount { get; set; }
        public string editrow { get; set; }
        public string view { get; set; }
        public string move { get; set; }
        public string delrow { get; set; }
        public string report { get; set; }
        public string report1 { get; set; }

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
