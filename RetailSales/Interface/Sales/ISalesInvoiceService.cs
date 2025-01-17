using RetailSales.Models;
using System.Data;

namespace RetailSales.Interface.Sales
{
    public interface ISalesInvoiceService
    {
        DataTable GetAllSalesInvoice(string strStatus);
        DataTable GetSalesInvoice(string id);
        string InvoicetoReturn(string id);
    }
}
