using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ICustomerService
    {
        string CustomerCRUD(Customer cy);
        DataTable GetAllCustomerGRID(string strStatus);
        DataTable GetEditCustomerDetail(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);


        //DataTable GetCustomercategory();
        DataTable GetCustomercategory();
        DataTable GetCountry();
        DataTable GetState();
        DataTable GetCity();
      
    }
}
