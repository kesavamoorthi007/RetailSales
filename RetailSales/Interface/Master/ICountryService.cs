using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ICountryService
    {
        string CountryCRUD(Country ic);
        DataTable GetAllCountryGRID(string strStatus);
        DataTable GetEditCountryDetail(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
    }
}
