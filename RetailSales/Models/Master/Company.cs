using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Company
    {
        public string ID { get; set; }
        public string Code {  get; set; }
        public string CompanyName {  get; set; }
        public string Country {  get; set; }
        public string State {  get; set; }
        public string City {  get; set; }
        public string Address {  get; set; }
        public string Mobile {  get; set; }
        public string Landline {  get; set; }
        public string Fax {  get; set; }
        public string Email {  get; set; }
        public string CST {  get; set; }
        public string ddlStatus {  get; set; }
    }
    public class Companygrid
    {
        public string id { get; set; }
        public string coname { get; set; }
        public string concode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
