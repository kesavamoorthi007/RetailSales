namespace RetailSales.Models.Master
{
    public class TaxMaster
    {
        public string ID { get; set; }
        public string TaxName {get; set; }
        public string Percentage { get; set; }
        public string Taxdescription { get; set; }
        public string CreatedBy { get; set;}
        public string CreatedOn { get; set;}
        public string UpdatedBy { get; set;}
        public string UpdatedOn { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string ddlstatus { get; set; }
    }
    public class ListTaxMasterGrid
    {
        public string id { get; set; }
        public string taxname { get; set; }
        public string percentage { get; set; }
        public string taxdescription { get; set; }
        public string createdby { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
