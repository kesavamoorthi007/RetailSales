using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Supplier
    {
        public Supplier()
        {
            this.Countrylst = new List<SelectListItem>();
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();
            this.Categorylst = new List<SelectListItem> ();
        }
        public List<SelectListItem> Countrylst;
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public List<SelectListItem> Categorylst;
        public string ID { get; set; }
        public string Supp { get; set; }
        public string Category { get; set; }
       
        public string Days { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Gst { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ddlstatus { get; set; }
    }

    public class ListSuppliergrid
    {
        public string id { get; set; }
        public string supp { get; set; }
        public string category { get; set; }
        
        public string days { get; set; }
        public string city { get; set; }
        public string state { get; set; }
       
        public string gst { get; set; }
        public string mobile { get; set; }
        public string landline { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
