using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ICompanyService
    {
        string CompanyCRUD(Company cy);
        DataTable GetAllCompanyGRID(string strStatus);
        DataTable GetEditCompanyDetail(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
    }
}
