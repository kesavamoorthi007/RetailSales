using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Purchase
{
    public interface IDirectPurchaseService
    {
        DataTable GetItem();
        DataTable GetVariant(string id);
        DataTable GetSupplierDetails(string id);
        DataTable GetVarientDetails(string id);
        DataTable GetHsn(string id);
        DataTable GethsnDetails(string id);
        DataTable GetgstDetails(string id);
        string DirectPurchaseCRUD(DirectPurchase cy);

        DataTable GetAllListDirectPurchase(string strStatus);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetEditDirectPurchase(string id);
        DataTable GetEditDirectPurchaseItem(string id);
        DataTable GetDirectPurchase(string id);
        DataTable GetDirectPurchaseItem(string id);
        string SupplierCRUD(string SupplierName, string SupplierAdd, string State, String City);
    }
}
