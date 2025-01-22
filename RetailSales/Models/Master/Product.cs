using Microsoft.AspNetCore.Mvc.Rendering;


namespace RetailSales.Models.Master
{
    public class Product
    {
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string ddlStatus { get; set; }
    }
    public class Productgrid
    {
        public string id { get; set; }
        public string product { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
