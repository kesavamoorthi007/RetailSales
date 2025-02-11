using System.Data;

namespace RetailSales.Interface.Inventory
{
    public interface IStockAdjustmentService
    {
        DataTable GetLocation();
        DataTable GetItem();
        DataTable GetVariant(string id);
        DataTable GetVariantDetails(string id);
        DataTable GetEditStockAdjustmentItem(string id);
        DataTable GetAllStockAdjustment(string strStatus);
        //DataTable GetAllStockAdjustment(string strStatus);
        //DataTable GetEditStockAdjustment(string id);
        //DataTable GetEditStockAdjustmentItem(string id);
    }
}
