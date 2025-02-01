using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IProductdetailService
    {
        DataTable GetAllProductDeatilsGRID(string strStatus);
        DataTable GetCategory();
        DataTable GetEditProductdetail(string id);
        DataTable GetUom();
        string ProductdetailCRUD(Productdetail cy);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
    }
}


