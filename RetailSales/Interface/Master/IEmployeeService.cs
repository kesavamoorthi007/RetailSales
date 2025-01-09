using RetailSales.Models;
using System.Data;


namespace RetailSales.Interface.Master
{
    public interface IEmployeeService
    {
        string EmployeeCRUD(Employee Ic);
        DataTable GetAllEmployeeGRID(string strStatus);
        DataTable GetEditEmployeeDetail(string id);
        string StatusDeleteMR(string tag, int id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
    }
}
