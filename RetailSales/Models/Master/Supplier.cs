namespace RetailSales.Models
{
    public class Supplier
    {
        public string ID { get; set; }
        public string Supp { get; set; }
        public string Category { get; set; }
        public string Delivery { get; set; }
        public string Days { get; set; }
        public string City { get; set; }
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
        public string delivery { get; set; }
        public string days { get; set; }
        public string city { get; set; }
        public string mobile { get; set; }
        public string landline { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
