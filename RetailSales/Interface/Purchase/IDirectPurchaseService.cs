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
    }
}
