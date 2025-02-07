using System.Data;

namespace RetailSales.Interface.Inventory
{
    public interface IStockAdjustmentService
    {
        DataTable GetItem();
        DataTable GetItemDetails(string id);
        DataTable GetItemVariant();
    }
}
