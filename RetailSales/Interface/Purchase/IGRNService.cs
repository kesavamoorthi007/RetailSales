using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Purchase
{
    public interface IGRNService
    {
        DataTable GetAllListGRN(string strStatus);
        DataTable GetGodownBin();
        DataTable GetGRN(string id);
        DataTable GetGRNItem(string id);
        DataTable GetShopBin();
        string StatusChange(string tag, string id);
        string GRNACCOUNT(GRN cy);
        string StockAdjust(GRN cy);
    }
}
