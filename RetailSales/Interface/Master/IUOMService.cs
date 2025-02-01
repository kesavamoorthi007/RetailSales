using System.Data;
using RetailSales.Models.Master;

namespace RetailSales.Interface.Master
{
    public interface IUOMService
    {
        DataTable GetEditUOM(string id);
        DataTable GetAllUOMGRID(string strStatus);
        string UOMCRUD(UOM cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
    }
}
