using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Master
{
    public class ProductName
    {
        public ProductName()
        {
            this.Categorylst = new List<SelectListItem>();
        }
        public List<SelectListItem> Categorylst;
        public string ID { get; set; }
        public string Category { get; set; }
        public string ProName { get; set; }
        public string Description { get; set; }
        public string ddlstatus { get; set; }
    }

    public class ProsuctNamegrid
    {
        public string id { get; set; }
        public string category { get; set; }
        public string proname { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
