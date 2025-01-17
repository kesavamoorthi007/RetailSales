using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ICGroupService
    {
        string CGroupCRUD(CGroup Ic);
        DataTable GetAllCGroupGRID(string strStatus);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetEditCGroupDetail(string id);
    }
}
