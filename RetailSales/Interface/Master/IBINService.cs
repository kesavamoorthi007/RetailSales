using System.Data;
using RetailSales.Models.Master;

namespace RetailSales.Interface.Master
{
    public interface IBINService
    {
        string BINCRUD(BIN cy);
        public DataTable GetAllBINGRID(string strStatus);
        DataTable GetEditBIN(string id);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
    }
}
