using System.Data;

namespace RetailSales.Interface.Accounts
{
    public interface IAccountGroupService
    {
        public DataTable GetEditAccountGroupDetail(string id);
        public DataTable GetAllAccountGroupGRID(string strStatus);


        DataTable GetAccountClass();

        DataTable GetAccountType();

    }
}
