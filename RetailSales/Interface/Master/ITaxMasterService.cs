using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface ITaxMasterService
    {
        public DataTable GetEditTaxMaster(string id);
        public DataTable GetAllTaxMaster(string strStatus);
        string TaxMasterCRUD(TaxMaster cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
    }
}
