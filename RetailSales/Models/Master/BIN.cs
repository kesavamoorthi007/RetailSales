using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Master
{
    public class BIN
    {

        //public BIN()
        //{
        //    this.locationlist = new List<SelectListItem>();
        //}
        //public List<SelectListItem> locationlist;

        public string ID { get; set; }
        public string BINID { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Createdby { get; set; }
        public string Createdon { get; set; }
        public string Updatedby { get; set; }
        public string Updatedon { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string ddlstatus { get; set; }
    }

    public class ListBIN
    {
        public string id { get; set; }
        public string binid { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string edit { get; set; }
        public string delete { get; set; }
    }
}
