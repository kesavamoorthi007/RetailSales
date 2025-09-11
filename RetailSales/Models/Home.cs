namespace RetailSales.Models
{
    public class Home
    {
        public string ProductName { get; set; }
        public string Variant { get; set; }
        public int TotalQtySold { get; set; }
        public DateTime INV_DATE { get; set; }
        public int Total_Sales { get; set; }
        public decimal Total_Amount { get; set; }
        public string MonthYear { get; set; }

    }
}
