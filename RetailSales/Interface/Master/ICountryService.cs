using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ICountryService
    {
        string CountryCRUD(Country ic);
        DataTable GetAllCountryGRID(string strStatus);
    }
}
