using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models
{
    public class Productdetail
    {
        public Productdetail()
        {
            this.Categorylst = new List<SelectListItem>();
        }
        public List<SelectListItem> Categorylst;
        public string Product { get; set; }
        public string Varint { get; set; }
        public string Productdescription { get; set; }
        public string Hsncode { get; set; }
        public string Productlist { get; set; }
        public string Uom { get; set; }


    }
}
