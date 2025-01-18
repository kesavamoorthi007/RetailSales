namespace RetailSales.Models
{
    public class Stockinhand
    {

        public string ProductName { get; set; }
        public string Variant { get; set; }
        public string Balancequantity { get; set; }
        public string Uom { get; set; }
        public string UnitCost { get; set; }
        public string Totalvalue { get; set; }
        public string Isactive { get; set; }
        public string ddlStatus { get; set; }
        public string ID { get; internal set; }
    }
    public class Stockinhandgrid
    {
        public string id { get; set; }
        public string productname { get; set; }
        public string variant { get; set; }
        public string balancequantity { get; set; }
        public string uom { get; set; }
        public string unitcost { get; set; }
        public string totalvalue { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
