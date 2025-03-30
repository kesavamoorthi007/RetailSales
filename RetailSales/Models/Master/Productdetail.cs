using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Productdetail
    {
        public Productdetail()
        {
            this.Categorylst = new List<SelectListItem>();
            this.Productlst = new List<SelectListItem>();
            this.Uomlst = new List<SelectListItem>();
            this.Hsnlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Categorylst;
        public List<SelectListItem> Productlst;
        public List<SelectListItem> Uomlst;
        public List<SelectListItem> Hsnlst;
        public string ID { get; set; }
        public string Product { get; set; }
        public string Varint { get; set; }
        public string ProName { get; set; }
        public string Uom { get; set; }
        public string Hsncode { get; set; }
        public string Minqty { get; set; }
        public string Rate { get; set; }
        public string Productdescription { get; set; }
        public string ddlStatus { get; set; }
        public List<ProductDetailTable> ProductDetailTablelst { get; set; }

    }

    public class ProductDetailTable
    {
        public string ProID { get; set; }
        public List<SelectListItem> SUOMlst { get; set; }
        public string Src { get; set; }
        public List<SelectListItem> DUOMlst { get; set; }
        public string Des { get; set; }
        public string CF { get; set; }
        public string saveItemId { get; set; }
        public string Isvalid { get; set; }
    }

    public class Productdetailgrid
    {
        public string id { get; set; }
        public string product { get; set; }
        public string varint { get; set; }      
        public string proname { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string cf { get; set; }

    }
}
