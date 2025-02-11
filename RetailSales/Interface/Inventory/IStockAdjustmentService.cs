using System.Data;

namespace RetailSales.Interface.Inventory
{
    public interface IStockAdjustmentService
    {
        DataTable GetItem();
        DataTable GetVariantDetails(string id);
        DataTable GetVariant(string id);
        DataTable GetLocation();
    }
}
