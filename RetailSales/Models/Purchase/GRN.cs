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
    }
    public class ListGRNgrid
    {
        public string id { get; set; }
        public string grn { get; set; }
        public string grndate { get; set; }
        public string sup { get; set; }
        public string net { get; set; }
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
