using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Master
{
    public class Rate
    {
        public string ID { get; set; }
        //public string po { get; set; }
        //public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public List<RateList> RateListIdem { get; set; }
        public string ddlStatus { get; set; }
    }

    public class RateList
    {
        public string Isvalid { get; set; }
        public string ID { get; set; }
        public string Item { get; set; }
        public string Varient { get;set; }
        //public List<SelectListItem> Itemlst { get; set; }
        //public List<SelectListItem> Variantlst { get; set; }
        public string saveItemId { get; set; }
        public string Unit { get; set; }
        public string Rate1 { get; set; }
    }
    
    public class RateGrid
    {
        public string id { get; set; }
        public string docno { get; set; }
        public string docdate { get; set; }
        public string validfrom { get; set; }
        public string validto { get; set; }
        public string editrow { get; set; }
        public string revision { get; set; }
        public string delrow { get; set; }

    }
}
