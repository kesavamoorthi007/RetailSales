using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Purchase
{
    public interface IPurchaseorderService
    {
        DataTable GetAllListPurchaseorder(string strStatus);
        DataTable GetSupplierDetails(string id);
        DataTable GetItem();
        DataTable GetVariant(string id);
        DataTable GetVarientDetails(string id);
        string PurchaseorderCRUD(Purchaseorder cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetPurchasOrder(string id);
        DataTable GetPurchasOrderItem(string id);
    }
}
