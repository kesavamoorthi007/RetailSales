using RetailSales.Models;
using System.Data;


namespace RetailSales.Interface.Master
{
    public interface ICCategoryService
    {
        DataTable GetAllCCategoryGRID(string strStatus);
        DataTable GetEditCCategoryDetail(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        string CCategoryCRUD(CCategory cy);
    }
}
