using RetailSales.Models;
using System.Data;
namespace RetailSales
{
    public interface IStockTransferService
    {
        
        DataTable GetFBin();
        DataTable GetTBin();
     

        string StockTransferCRUD(StockTransfer ic);
        DataTable GetAllStockTransferGRID(string strStatus);
        DataTable GetItemDetails(string itemId);
        DataTable GetVarientDetails(string itemId);
        DataTable GetVariant(string id);
        DataTable GetEditStockTransferDetail1(string id);
        //DataTable GetStockTransferItem(string id);
        string StatusChange(string tag, string id);
        DataTable GetItem();
        //string StatusChange(string tag, string id);
        //string RemoveChange(string tag, string id);
        //string StatusDeleteMR(string tag, int id);
    }
}

