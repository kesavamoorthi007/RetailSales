using System.Data;

namespace RetailSales.Interface.Purchase
{
    public interface IPurchaseorderService
    {
        DataTable GetAllListPurchaseorder(string strStatus);
    }
}
