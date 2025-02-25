using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class SalesReturn
    {
        public string ID { get; set; }
        public string ddlStatus { get; set; }
    }
    public class SalesReturngrid
    {
        public string id { get; set; }
        public string doc { get; set; }
        public string date { get; set; }
        public string invno { get; set; }
        public string invdate { get; set; }
        public string customer { get; set; }
        public string type { get; set; }
        public string go { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
        
    }
}
