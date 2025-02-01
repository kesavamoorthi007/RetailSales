using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Productdetail
    {
        public Productdetail()
        {
            this.Categorylst = new List<SelectListItem>();
            this.Uomlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Categorylst;
        public List<SelectListItem> Uomlst;
        public string Product { get; set; }
        public string Varint { get; set; }
        public string Varintnic { get; set; }
        public string Productdescription { get; set; }
        public string Hsncode { get; set; }
        public string Productlist { get; set; }
        public string Uom { get; set; }
        public string Rate { get; set; }
        public string ddlStatus { get; set; }

        public string? ID { get; internal set; }

        public string minqty { get; set; }

    }
    public class Productdetailgrid
    {
       
        public string id { get; set; }
        public string product { get; set; }
        public string varint { get; set; }
        public string varintnic { get; set; }
        public string uom { get; set; }
        public string rate { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
