using RetailSales.Models;
using System.Collections.Generic;
using System.Collections;
using RetailSales.Interface.Master;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IHSNcodeService
    {
        string HSNcodeCRUD(HSNcode by);
        //IEnumerable<HSNcode> GetAllHSNcode(string status);
        //HSNcode GetHSNcodeById(string id);

        //DataTable GetCGst();

        //DataTable GetSGst();
        //DataTable GetIGst();

       
        DataTable GettariffItem(string id);

        DataTable GetHSNcode(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

        DataTable GetAllhsncode(string strStatus);
       
        DataTable Gettariff();

    }
}
