using RetailSales.Models;
using System.Data;
namespace RetailSales
{
    public interface IStockTransferService
    {
        DataTable GetFlocation();
        DataTable GetTlocation();
        DataTable GetFBin();
        DataTable GetTBin();
        DataTable Item { get; }

        DataTable GetItemDetails(string itemId);
    }
}

