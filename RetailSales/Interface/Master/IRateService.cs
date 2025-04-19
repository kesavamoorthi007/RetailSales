using RetailSales.Models.Master;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IRateService
    {
        //public DataTable GetItem();

        //public DataTable GetItemDetails(string ItemId);

        public DataTable GetAllRate(string strStatus);

        DataTable GetproductDetail(string id);

        string RateCRUD(Rate cy, string proid);

        public DataTable GetEditRate(string id);

        public DataTable GetEditRateDetail(string id);

        public string RemoveChange(string tag, string id);

        public string StatusChange(string tag, string id);
        DataTable GetRateView(string id);
        DataTable GetRateViewTable(string id);
        DataTable GetUom();

        //public DataTable GetVariant(string id);

    }
}
