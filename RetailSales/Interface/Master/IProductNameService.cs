using System.Data;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Interface.Master
{
    public interface IProductNameService
    {
        DataTable GetAllProductNameGRID(string strStatus);
        DataTable GetCategory();
        DataTable GetEditProductName(string id);
        DataTable GetEditProductNameItem(string id);
        DataTable GetHsn();
        DataTable GetProductName(string id);
        DataTable GetProductNameItem(string id);
        DataTable GetUom();
        string ProductNameCRUD(ProductName cy);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
        string CFCRUD(Productdetail cy, string proid);
    }
}
