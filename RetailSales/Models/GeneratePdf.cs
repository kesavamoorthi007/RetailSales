using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class GeneratePdf
    {
        public string ID { get; set; }
        public string PoNo { get; set; }
        public string PoDate { get; set; }
        public string CompName { get; set; }
        public string CompAdd { get; set; }
        public List<SelectListItem> Suplst { get; set; }
        public string SuppName { get; set; }
        public string SuppAdd { get; set; }
        public string GSTNo { get; set; }
        
        public List<GeneratePdfItem> GeneratePdfLst { get; set; }
    }

    public class GeneratePdfItem
    {
        public string ID { get; set; }
        public string Category { get; set; }
        public string Product { get; set; }
        public string Variant { get; set; }
        public string Qty { get; set; }
        public string Isvalid { get; set; }
    }
}
