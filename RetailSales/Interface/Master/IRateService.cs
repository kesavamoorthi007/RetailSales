using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IRateService
    {
        public DataTable GetItem();

        public DataTable GetItemDetails(string ItemId);
    }
}
