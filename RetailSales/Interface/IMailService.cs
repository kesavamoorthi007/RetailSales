using RetailSales.Models;
using System.Data;
namespace RetailSales.Interface
{
    public interface IMailService
    {
        void SendEmailAsync(MailRequest mailRequest);

    }
}
