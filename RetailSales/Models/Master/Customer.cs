using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Customercategorylst = new List<SelectListItem>();
            this.Countrylst = new List<SelectListItem>();
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();

        }
        public List<SelectListItem> Customercategorylst;
        public List<SelectListItem> Countrylst;
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public string ID { get; set; }
        public string Category { get; set; }
        public string Customername { get; set; }
        public string CustomerCode { get; set; }
        public string Customercategory { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Gst { get; set; }
        public object CustomerName { get; internal set; }
        //public object Customercategory { get; internal set; }
        public string ddlStatus { get; set; }
    }
    public class Customergrid
    {
        public string id { get; set; }
        public string ctname { get; set; }
        public string ctcode { get; set; }
        public string category { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
