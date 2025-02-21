using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Models
{
    public class PromotionMail
    {
        //internal IEnumerable<Customeremailattach> Upload { get; set; }

        public string ID { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Sub { get; set; }
        //public IFormFile Attachment { get; set; }
        public string editors { get; set; }
        public string? FileName { get; internal set; }
        public Stream InputStream { get; internal set; }
    }
    //public class Customeremailattach
    //{
    //    public string ID { get; set; }
    //    public string Isvalid { get;  set; }
    //    public string Upload { get; set; }
    //    public string FilePath { get; set; }
    //}
}
