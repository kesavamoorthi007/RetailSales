using System.Data;

namespace RetailSales.Interface.Master
{
    public interface IProductdetailService
    {
        DataTable GetAllProductDeatilsGRID();
        DataTable GetCategory();
        DataTable GetUom();
    }
}
