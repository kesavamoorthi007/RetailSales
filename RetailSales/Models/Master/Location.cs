using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Location
    {
        public string ID { get; set; }
        public string Locationname { get; set; }
        public string Address { get; set; }
        public string Bin { get; set; }
        public string ddlStatus { get; set; }
    }
    public class Locationgrid
    {
        public string id { get; set; }
        public string lname { get; set; }
        public string address { get; set; }
        public string bin { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
