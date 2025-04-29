using System.Data;
using RetailSales.Models.Accounts;

namespace RetailSales.Interface.Accounts
{
    public interface IPaymentRequestService
    {
        DataTable GetAllListPurchaseorder(string strStatus);
        DataTable GetEditPaymentRequest(string id);
        DataTable GetEditPaymentRequestTable(string id);
        DataTable GetGRNDetails(string ItemId);
        DataTable GetPaymentRequest(string id);
        DataTable GetPaymentRequestItem(string id);
        DataTable GetPaymentRequestTable(string id);
        string PaymentRequestCRUD(PaymentRequest cy);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);
    }
}
