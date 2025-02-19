namespace RetailSales.Models.Master
{
    public class EmailConfig
    {
        public string ID { get; set; }
        public string Smtphost { get; set; }
        public string Portno { get; set; }
        public string Emailid { get; set; }
        public string Password { get; set; }
        public string SSL { get; set; }
        public string Signature { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string ddlstatus { get; set; }
    }

    public class ListEmailConfig
    {
        public string id { get; set; }
        public string smtphost { get; set; }
        public string portno { get; set; }
        public string emailid { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
