using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Sales
{
    public interface ISalesInvoiceService
    {
        DataTable GetAllSalesInvoice(string strStatus);
        DataTable GetItem();
        DataTable GetVariant(string id);
        DataTable GetVarientDetails(string id);
        DataTable GetSalesInvoice(string id);
        string InvoicetoReturn(string id);
        DataTable GetSalesInvoiceItem(string id);
        Task<IEnumerable<ExinvBasicItem>> GetBasicItem(string id);
        Task<IEnumerable<ExinvDetailitem>> GetExinvItemDetail(string id);
        DataTable GetState();
        DataTable GetCity(string cityid);
        DataTable GetUOM();
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetEditSalesInvoice(string id);
        DataTable GetEditSalesInvoiceItem(string id);
        string SalesInvoiceCRUD(SalesInvoice cy);
        DataTable GetStockDetails(string ItemId);
        DataTable GetProduct(string id);
        //string SalesInvoiceCRUD(Purchaseorder cy);
    }
}
