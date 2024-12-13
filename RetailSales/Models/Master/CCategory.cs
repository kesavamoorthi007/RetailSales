using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class CCategory
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ddlStatus { get; set; }
    } 
    public class CCategorygrid
    {
        public string id { get; set; }
        public string cate { get; set; }
        public string des { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
