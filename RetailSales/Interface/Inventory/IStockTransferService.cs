using RetailSales.Models;
using System.Data;
namespace RetailSales
{
    public interface IStockTransferService
    {
        
        DataTable GetFBin();
        DataTable GetTBin();
        DataTable GetItem();
        DataTable GetVariant(string id);


        string StockTransferCRUD(StockTransfer ic);
        DataTable GetAllStockTransferGRID(string strStatus);
        DataTable GetItemDetails(string itemId);
        DataTable GetVarientDetails(string itemId);
        
        DataTable GetEditStockTransferDetail1(string id);
    
        string StatusChange(string tag, string id);
    
        string RemoveChange(string tag, string id);
        DataTable GetStockTransfer(string id);
        DataTable GetStockTransferItem(string id);
        DataTable GetEditStockTransfer(string id);
        DataTable GetProduct(string id);
        DataTable GetStockDetails(string ItemId, string floc);
        DataTable GetFLocation();
        DataTable GetTLocation();
        DataTable GetFBinDetails(string ItemId);
    }
}

