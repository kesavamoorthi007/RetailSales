using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Bankaccounts
    {
        public Bankaccounts()
        {
            this.Accounttypelst = new List<SelectListItem>();
            this.Countrylst = new List<SelectListItem>();
            this.Statelst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();

        }
        public List<SelectListItem> Accounttypelst;
        public List<SelectListItem> Countrylst;
        public List<SelectListItem> Statelst;
        public List<SelectListItem> Citylst;
        public string Accountname { get; set; }
        public string Bankname { get; set; }
        public string Accounttype { get; set; }
        public string Branchname { get; set; }
        public string Branchaddress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Bsrcode { get; set; }
        public string Ifsccode { get; set; }
        public bool Isdefault { get; set; }
        public string Openingbalance { get; set; }
        

    }
}
