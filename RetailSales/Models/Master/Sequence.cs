namespace RetailSales.Models
{
    public class Sequence
    {
        public string ID { get; set; }
        public string Transection { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Lnumber { get; set; }
        public string Number { get; set; }
        public string ddlStatus { get; set; }
    }
    public class Sequencegrid
    {
        public string id { get; set; }
        public string transection { get; set; }
        public string prefix { get; set; }
        public string suffix { get; set; }
        public string lnumber { get; set; }
        public string number { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
