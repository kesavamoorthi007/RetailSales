using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Purchaseorder
    {
        public Purchaseorder()
        {
            this.Suplst = new List<SelectListItem>();
        }
        public string Companyname { get; set; }
        public string Suppid { get; set; }
        public List<SelectListItem> Suplst { get; set; }
        public string refno { get; set; }
        public string refdate { get; set; }
        public string Podate { get; set; }
        public string Supplieraddress { get; set; }

        public string ddlStatus { get; set; }
        public List<PurchaseorderItem> PurchaseorderLst { get; set; }
        public string Narration { get; set; }
        public string Amountinwords { get; set; }
        public string TotDis { get; set; }
        public string Total { get; set; }
        public string Tot { get; set; }

    }
    public class PurchaseorderItem
    {
        public string Isvalid { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string Discount { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }

    }
    public class ListPurchaseordergrid
    {
        public string id { get; set; }
        public string doc { get; set; }
        public string docdate { get; set; }
        public string company { get; set; }
        public string suppier { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
