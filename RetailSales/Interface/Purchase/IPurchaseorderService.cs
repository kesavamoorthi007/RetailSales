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
        DataTable GetDUOM();
        DataTable GetVarientDetails(string id);
        string PurchaseorderCRUD(Purchaseorder cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetPurchasOrder(string id);
        DataTable GetEditPurchasOrder(string id);
        DataTable GetPurchasOrderItem(string id);
        DataTable GetEditPurchasOrderItem(string id);
        DataTable GetHsn(string id);
        DataTable GethsnDetails(string id);
        DataTable GetgstDetails(string id);
        string OrderToGRN(Purchaseorder cy);
        DataTable GetUOM();
        IEnumerable<PurchaseorderItem> GetAllPurchaseOrderItem(string id);
        DataTable GetProduct(string productid);
        DataTable GetState();
        DataTable GetCity(string cityid);
        DataTable GetCategory();
        string SupplierCRUD(string category, string supplierName, string supplierAdd, string days, string gST, string state, string city, string mobile, string landline, string email);
        DataTable GetHsn();
        string ProductNameCRUD(Purchaseorder cy);
        DataTable GetProdCategory();
        DataTable GetGeneratePdf(string id);
        DataTable GetGeneratePdfItem(string id);
        //string SupplierCRUD(Purchaseorder cy);
        //string ProductNameCRUD(string category, string product, string description, string varient, string uOM, string hsn, string minQty, string rate, string prodDesc);
    }
}
