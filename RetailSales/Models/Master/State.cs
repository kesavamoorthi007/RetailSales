using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Models.Master
{
    public class State
    {
        public State()
        {

            this.Cuntylst = new List<SelectListItem>();
        }

        public string ID { get; set; }
        public string StatCode { get; set; }
        public string StatName { get; set; }
        public string ddlStatus { get; set; }
        public string ConName { get; set; }

        public List<SelectListItem> Cuntylst;


    }

    public class Stategrid
    {
        public string id { get; set; }
        public string statcode { get; set; }
        public string statname { get; set; }
        public string conname { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
