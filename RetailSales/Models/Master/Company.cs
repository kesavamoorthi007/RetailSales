using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Company
    {
        public Company()
        {
            this.Countrylst = new List<SelectListItem>();
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();

        }
        public List<SelectListItem> Countrylst;
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public string ID { get; set; }
        public string CompanyName {  get; set; }
        public string Address {  get; set; }
        public string Mobile {  get; set; }
        public string Country {  get; set; }
        public string State {  get; set; }
        public string City {  get; set; }
        public string Email {  get; set; }
        public string Landline {  get; set; }
        public string Website {  get; set; }
        public string ddlStatus {  get; set; }

       
    }
    public class Companygrid
    {
        public string id { get; set; }
        public string coname { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
