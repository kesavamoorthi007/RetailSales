using System.Data;
using RetailSales.Models;

namespace RetailSales.Interface.Master
{
    public interface ISupplierService
    {
        public DataTable GetEditSupplier(string id);
        public DataTable GetAllSupplierGRID(string strStatus);
        string SupplierCRUD(Supplier cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

       
        DataTable GetState();
        DataTable GetCity();
        DataTable GetCategory();
    }
}
