using System.ComponentModel.DataAnnotations;

namespace RetailSales.Models.Master
{
    public class UOM
    {       
        public string ID { get; set; }
        public string UOMCODE { get; set; }       
        public string Description { get; set; }
        public string Factor { get; set; }
        public string Createdby { get; set; }
        public string Createdon { get; set; }
        public string Updatedby { get; set; }
        public string Updatedon { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string ddlstatus { get; set; }
    }

    public class ListUOMGrid
    {
        public string id { get; set; }
        public string uomcode { get; set; }
        public string description { get; set; }
        public string factor { get; set; }

        public string edit { get; set; }
        public string delete { get; set; }
    }
}
