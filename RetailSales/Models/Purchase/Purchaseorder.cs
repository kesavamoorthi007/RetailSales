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
        public string po { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
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
        public string ID { get; set; }
        public string Gross { get; set; }
        public string Disc { get; set; }
        public string GST { get; set; }
        public string Frieghtcharge { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Round { get; set; }
        public string Net { get; set; }
        public string drivername { get; set; }
        public string LRno { get; set; }
        public string dispatchname { get; set; }
        public string LRdate { get; set; }
    }
    public class PurchaseorderItem
    {
        public List<SelectListItem> Itemlst { get; set; }
        public string saveItemId { get; set; }
        public string Item { get; set; }
        public List<SelectListItem> Varientlst { get; set; }
        public string Varient { get; set; }

        public List<SelectListItem> UOMlst { get; set; }
        public string DestUOM { get; set; }
        public string CF { get; set; }
        public string CfQty { get; set; }
        public string Isvalid { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string Recived { get; set; }
        public string Accepted { get; set; }
        public string Rejected { get; set; }
        public string exqty { get; set; }
        public string shortqty { get; set; }
        public string DiscAmount { get; set; }
        public string FrigCharge { get; set; }
        public string FrieghtAmt { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Hsn { get; set; }
        public string ID { get; set; }
        public string Tariff { get; set; }
        public string Bin { get; set; }
        public string CGSTP { get; set; }
        public string SGSTP { get; set; }
        public string IGSTP { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Total { get; set; }


    }
    public class ListPurchaseordergrid
    {
        public string id { get; set; }
        public string po { get; set; }
        public string podate { get; set; }
        public string sup { get; set; }
        public string refno { get; set; }
        public string mailrow { get; set; }
        public string pdf { get; set; }
        public string editrow { get; set; }
        public string move { get; set; }
        public string view { get; set; }
        public string delrow { get; set; }

    }
}
