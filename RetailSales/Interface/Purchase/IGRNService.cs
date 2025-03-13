using System.Data;

namespace RetailSales.Interface.Purchase
{
    public interface IGRNService
    {
        DataTable GetAllListGRN(string strStatus);
        string StatusChange(string tag, string id);
    }
}
