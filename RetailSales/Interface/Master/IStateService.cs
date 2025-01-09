using System.Data;
using RetailSales.Models.Master;

namespace RetailSales.Interface.Master
{
    public interface IStateService
    {
        string StateCRUD(State Ic);
        DataTable GetEditStateDetail(string id);
        DataTable GetAllStateGRID(string strStatus);

        DataTable GetCountry();

        DataTable GetState();



        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
      
      
    }
}
