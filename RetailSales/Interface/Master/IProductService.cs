using RetailSales.Models.Master;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IProductService
    {
        DataTable GetAllProductGRID(string strStatus);
        DataTable GetEditProductDetail(string id);
        string ProductCRUD(Product cy);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
    }
}
