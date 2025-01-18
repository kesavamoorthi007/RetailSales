namespace RetailSales.Models
{
    public class Purchaseorder
    {
        public string Companyname { get; set; }
        public string Suppliername { get; set; }
        public string Deliveryaddress { get; set; }
        public string Supplierlocation { get; set; }
        public string Purchaseorderdate { get; set; }
        public string Supplieraddress { get; set; }

        public string ddlStatus { get; set; }
        public List<PurchaseorderItem> PurchaseorderLst { get; set; }

    }
    public class PurchaseorderItem
    {
        public string Isvalid { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string Discount { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }

    }
    public class ListPurchaseordergrid
    {
        public string id { get; set; }
        public string doc { get; set; }
        public string docdate { get; set; }
        public string company { get; set; }
        public string suppier { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
