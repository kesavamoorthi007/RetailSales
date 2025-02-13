using System.Data;
using RetailSales.Models.Accounts;

namespace RetailSales.Interface.Accounts
{
    public interface IAccountGroupService
    {
        public DataTable GetEditAccountGroupDetail(string id);
        public DataTable GetAllAccountGroupGRID(string strStatus);
        string AccountGroupCRUD(AccountGroup cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetAccountClass();
        DataTable GetAccountType();
        DataTable GetDaydet();
        
    }
}
