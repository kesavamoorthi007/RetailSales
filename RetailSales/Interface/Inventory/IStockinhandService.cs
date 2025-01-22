
using System.Data;

namespace RetailSales.Interface
{
    public interface IStockinhandService
    {
        //string Stockinhand(object iD);
        DataTable GetAllListStockinhand(string iD);
    }
}
