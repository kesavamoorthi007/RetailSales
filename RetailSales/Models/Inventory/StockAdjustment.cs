using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Inventory
{
    public class StockAdjustment
    {
        public StockAdjustment()
        {
            this.Locationlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Locationlst;

        public string ID { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string Reason { get; set; }
        public string Item { get; set; }
        public string Variant { get; set; }
        public string Unit { get; set; }
        public string StockQty { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string ddlStatus { get; set; }
        public List<StockAdjustmentItem> StockAdjustmentList { get; set; }
    }

    public class StockAdjustmentItem
    {
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> Productlst { get; set; }
        public List<SelectListItem> Variantlst { get; set; }
        public string saveItemId { get; set; }
        public string Item { get; set; }
        public string Product { get; set; }
        public string Variant { get; set; }
        public string Unit { get; set; }
        public string StockQty { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Isvalid { get; set; }
        public string ID { get; set; }
    }

    public class ListStockAdjustmentgrid
    {
        public string id { get; set; }
        public string location { get; set; }
        public string type { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string view { get; set; }
        //public string edit { get; set; }
        //public string delete { get; set; }
    }

}
