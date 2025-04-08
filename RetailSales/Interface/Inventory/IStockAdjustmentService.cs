using System.Data;
using RetailSales.Models.Inventory;

namespace RetailSales.Interface.Inventory
{
    public interface IStockAdjustmentService
    {
        DataTable GetLocation();
        DataTable GetItem();
        DataTable GetVariant(string id);
        DataTable GetVariantDetails(string id);
        DataTable GetAllStockAdjustment(string strStatus);
        DataTable GetEditStockAdjustment(string id);
        DataTable GetEditStockAdjustmentItem(string id);
        string StockAdjustmentCRUD(StockAdjustment cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetStockAdjustment(string id);
        DataTable GetStockAdjustmentItem(string id);
        DataTable GetProduct(string id);
        DataTable GetStockDetails(string ItemId);
    }
}
