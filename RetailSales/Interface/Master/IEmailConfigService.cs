using System.Data;
using RetailSales.Models.Master;

namespace RetailSales.Interface.Master
{
    public interface IEmailConfigService
    {
        DataTable GetAllEmailConfigGRID(string strStatus);
        string EmailConfigCRUD(EmailConfig cy);
        DataTable GetEditEmailConfig(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);        
    }
}
