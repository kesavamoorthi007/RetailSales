using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class StockTransfer
    {
        public StockTransfer()
        {
            this.Flocationlst = new List<SelectListItem>();
            this.Tlocationlst = new List<SelectListItem>();
            this.FBinlst = new List<SelectListItem>();
            this.TBinlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Flocationlst;
        public List<SelectListItem> Tlocationlst;
        public List<SelectListItem> FBinlst;
        public List<SelectListItem> TBinlst;
        public string Documentid { get; set; }
        public string DocumentDate { get; set; }
        public string Flocation { get; set; }
        public string Tlocation { get; set; }
        public string FBin { get; set; }
        public string TBin { get; set; }
        public string Order { get; set; }
        public string ddlStatus { get; set; }

        public List<StockTransferItem> StockTransferItemLst { get; set; }

    }
    public class StockTransferItem
    {
        public string Isvalid { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Item { get; set; }
        public string saveItemId { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Bin { get; set; }
        public string Qty { get; set; }
        public string Discount { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Total { get; set; }
        //public string ID { get; set; }
        //public string ReturnQty { get; set; }
    }
}
