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
        public List<ProductNameItem> ProductNameLst { get; set; }
    }
    public class ProductNameItem
    {
        public string ID { get; set; }
        public string Variant { get; set; }
        public List<SelectListItem> UOMlst { get; set; }
        public string Uom { get; set; }
        public List<SelectListItem> HSNlst { get; set; }
        public string Hsn { get; set; }
        public string Rate { get; set; }
        public string MinQty { get; set; }
        public string ProdDesc { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> ShopBinlist { get; set; }
        public string ShopBin { get; set; }
        public List<SelectListItem> GodownBinlist { get; set; }
        public string GodownBin { get; set; }
    }

    public class ProsuctNamegrid
    {
        public string id { get; set; }
        public string category { get; set; }
        public string proname { get; set; }
        public string view { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
