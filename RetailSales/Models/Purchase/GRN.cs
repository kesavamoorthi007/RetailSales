using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class GRN
    {
        public string ID { get; set; }
        public string ddlStatus { get; set; }
        public string ADCOMPHID { get; set; }
        public string GRNID { get; set; }
        public string RefDate { get; set; }
        public string createdby { get; set; }
        public string Amtinwords { get; set; }
        public string Vmemo { get; set; }
        public string mid { get; set; }
        public List<GRNAccount> Acclst { get; set; }
        public double TotalNetmt { get; set; }
        public double Gross { get; set; }
        public double Net { get; set; }
        public double TotalCRAmt { get; set; }
        public double TotalDRAmt { get; set; }
        public double Frieghtcharge { get; set; }
        public double lrch { get; set; }
        public double Packingcharges { get; set; }
        public double Othercharges { get; set; }
        public double Round { get; set; }
        public double DiscAmt { get; set; }
        public double IGST { get; set; }
        public double SGST { get; set; }
        public double CGST { get; set; }


        public string GRNNo { get; set; }
        public string GRNDate { get; set; }
        public string SuppName { get; set; }
        public string SuppAdd { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string RefNo { get; set; }
        public string TransName { get; set; }
        public string PlaceOfDis { get; set; }
        public string LRNO { get; set; }
        public string LRDate { get; set; }
        public string AmtInWords { get; set; }
        public string Narration { get; set; }
        public string GrossAmt { get; set; }
        public string NetAmt { get; set; }
        public string Fright { get; set; }
        public string Cgst { get; set; }
        public string Sgst { get; set; }
        public string Igst { get; set; }
        public string RoundAmt { get; set; }
        public string Disc { get; set; }
        public List<GRNItem> GRNLst { get; set; }
    }

    public class GRNItem
    {
        public string ID { get; set; }
        public string Item { get; set; }
        public string Product { get; set; }
        public string Varient { get; set; }
        public string Hsn { get; set; }
        public string Tariff { get; set; }
        public string UOM { get; set; }
        public string Ordered { get; set; }
        public string Recived { get; set; }
        public string Accepted { get; set; }
        public string Rejected { get; set; }
        public string exqty { get; set; }
        public string shortqty { get; set; }
        public string DestUOM { get; set; }
        public string CF { get; set; }
        public string CfQty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string DiscPer { get; set; }
        public string DiscAmount { get; set; }
        public string CGSTP { get; set; }
        public string SGSTP { get; set; }
        public string IGSTP { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Total { get; set; }

    }

    public class ListGRNgrid
    {
        public string id { get; set; }
        public string grn { get; set; }
        public string grndate { get; set; }
        public string sup { get; set; }
        public string net { get; set; }
        public string accounts { get; set; }
        public string view { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        
    }
    public class GRNAccount
    {
        public string Ledgername { get; set; }
        public List<SelectListItem> Ledgerlist { get; set; }
        public string TypeName { get; set; }
        public bool IsDisable { get; set; }
        public string CRDR { get; set; }
        public List<SelectListItem> CRDRLst { get; set; }
        public double CRAmount { get; set; }
        //public bool IsDisable { get; set; }
        public double DRAmount { get; set; }
        public string Isvalid { get; set; }
        public string symbol { get; set; }
        public string mid { get; set; }
        public string crdrh { get; set; }

    }
}
