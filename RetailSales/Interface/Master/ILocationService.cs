using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ILocationService
    {
        string LocationCRUD(Location cy);
        DataTable GetAllLocationGRID(string strStatus);
        DataTable GetEditLocation(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

    }
}