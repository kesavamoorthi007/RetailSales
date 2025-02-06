using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Master
{
    public class Rate
    {
        public string ID { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public List<RateList> RateList { get; set; }
        public string ddlStatus { get; set; }
    }

    public class RateList
    {
        public string Isvalid { get; set; }
        public string Item { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string saveItemId { get; set; }
        public string Unit { get; set; }
        public string Rate1 { get; set; }
    }
}
