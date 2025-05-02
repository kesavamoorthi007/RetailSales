using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Master
{
    public class Rate
    {
        public string ID { get; set; }
        public string Product { get; set; }
        public string Varint { get; set; }
        public string ProName { get; set; }
        public string ddlStatus { get; set; }
        public List<RateListItem> RateListItemlst { get; set; }

    }

    public class RateListItem
    {
        public string ID { get; set; }
        public List<SelectListItem> UOMlst { get; set; }
        public string SrcUom { get; set; }
        public string SrcUomID { get; set; }
        public List<SelectListItem> DUOMlst { get; set; }
        public string DestUom { get; set; }
        public string CF { get; set; }
        public string ProdRate { get; set; }
        public string Percentage { get; set; }
        public string SalesRate { get; set; }
        public string Isvalid { get; set; }
    }

    public class RateGrid
    {
        public string id { get; set; }
        public string product { get; set; }
        public string varint { get; set; }
        public string proname { get; set; }
        public string cf { get; set; }

    }
}
