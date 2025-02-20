using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class StockTransfer
    {
        public StockTransfer()
        {

            this.FBinlst = new List<SelectListItem>();
            this.TBinlst = new List<SelectListItem>();

        }

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
        public string ID { get; internal set; }
        public List<StockTransferItem> StockTransferLst { get; internal set; }
    }
    public class StockTransferItem
    {
        public string Isvalid { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Item { get; set; }
        public string saveItemId { get; set; }
        public string Varient { get; set; }


        public string Unit { get; set; }
        public string Stock { get; set; }
        public string Qty { get; set; }

        public string Rate { get; set; }
        public string Amount { get; set; }

        public List<SelectListItem> Varientlst { get; internal set; }
        public string? Variant { get; internal set; }
        public string ID { get; internal set; }

        //public string ID { get; set; }
        //public string ReturnQty { get; set; }


        public class StockTransfergrid
        {
            public string id { get; set; }
            public string documentid { get; set; }
            public string stocktransferdate { get; set; }
            public string fromLoc { get; set; }
            public string toLoc { get; set; }
            public string fBin { get; set; }
            public string tBin { get; set; }
            public string view { get; set; }
            public string delrow { get; set; }
        }
    }
}
