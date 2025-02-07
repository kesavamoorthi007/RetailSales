using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Inventory
{
    public class StockAdjustment
    {
        public StockAdjustment()
        {
            this.Locationlist = new List<SelectListItem>();
        }
        public List<SelectListItem> Locationlist;

        public string ID { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string Reason { get; set; }
        public string Item { get; set; }
        public string ItemVariant { get; set; }
        public string Unit { get; set; }
        public string StockQty { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string ddlstatus { get; set; }
        public List<StockAdjustmentItem> StockAdjustmentList { get; set; }
    }

    public class StockAdjustmentItem
    {
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ItemVariantlst { get; set; }
        public string saveItemId { get; set; }
        public string Item { get; set; }
        public string ItemVariant { get; set; }
        public string Unit { get; set; }
        public string StockQty { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Isvalid { get; set; }
    }
}
