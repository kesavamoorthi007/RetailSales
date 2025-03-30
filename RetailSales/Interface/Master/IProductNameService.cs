using System.Data;
using RetailSales.Models.Master;

namespace RetailSales.Interface.Master
{
    public interface IProductNameService
    {
        DataTable GetAllProductNameGRID(string strStatus);
        DataTable GetCategory();
        DataTable GetEditProductName(string id);
        string ProductNameCRUD(ProductName cy);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
    }
}
