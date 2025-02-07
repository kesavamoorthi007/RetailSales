using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class DirectPurchase
    {
        public DirectPurchase()
        {
            this.Suplst = new List<SelectListItem>();
        }
        public string Suppid { get; set; }
        public List<SelectListItem> Suplst { get; set; }
        public string refno { get; set; }
        public string DocDate { get; set; }
        public string doc { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string refdate { get; set; }
        public string Supplieraddress { get; set; }

        public string ddlStatus { get; set; }
        public string Narration { get; set; }
        public string Amountinwords { get; set; }
        public string TotDis { get; set; }
        public string Total { get; set; }
        public string Tot { get; set; }
        public string ID { get; set; }
        public string Gross { get; set; }
        public string Disc { get; set; }
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
        public List<DirectPurchaseItem> DirectPurchaseLst { get; set; }
    }
    public class DirectPurchaseItem
    {
        public List<SelectListItem> Itemlst { get; set; }
        public string saveItemId { get; set; }
        public string Item { get; set; }
        public List<SelectListItem> Varientlst { get; set; }
        public string Varient { get; set; }
        public string Isvalid { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string Discount { get; set; }
        public string Rate { get; set; }
        public string Disc { get; set; }
        public string Amount { get; set; }
        public string DiscAmount { get; set; }
        public string Cf { get; set; }
        public string Hsn { get; set; }
        public string ID { get; set; }
        public string ReturnQty { get; set; }
        public string Tariff { get; set; }
        public string Bin { get; set; }
        public string Damount { get; set; }
        public string Other { get; set; }
        public string CGSTP { get; set; }
        public string SGSTP { get; set; }
        public string IGSTP { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Total { get; set; }
    }
}
