using DocumentFormat.OpenXml.Bibliography;
using System.Data;
using RetailSales.Models;

namespace RetailSales.Interface
{
    public interface ICityService
    {

        // retrieving from database
        DataTable GetAllCityGRID(string strStatus);

        // edit and store to database
        DataTable GetEditCityDetail(string id);

        // data deletion. data moves from enabled to disabled page
        string StatusChange(string tag, string id);

        // deleted data inclusion. data moves from disabled to enabled page
        string RemoveChange(string tag, string id);

        // getting i/p and storing to database
        string CityCRUD(Models.City ic);

        // used for country binding and retrieving from database
        DataTable GetCountry();

        // used for state binding and retrieving from database
        DataTable GetState(string stateid);

    }
}
