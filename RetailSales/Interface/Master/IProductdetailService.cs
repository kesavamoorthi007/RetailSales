using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IProductdetailService
    {
        DataTable GetAllProductDeatilsGRID(string strStatus);
        DataTable GetEditProductdetail(string id);
        DataTable GetEditProductdetailTable(string id);
        string ProductdetailCRUD(Productdetail cy);
        string CFCRUD(Productdetail cy, string id);
        DataTable GetCategory();
        DataTable GetUom();
        DataTable GetHsn();
        DataTable GetProductdetail(string id);
        DataTable GetProductdetailTable(string id);
        DataTable GetSUOM();
        DataTable GetDUOM();
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
        DataTable GetProduct(string productid);
    }
}


