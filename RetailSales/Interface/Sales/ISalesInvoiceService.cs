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
        //string SalesInvoiceCRUD(Purchaseorder cy);
    }
}
