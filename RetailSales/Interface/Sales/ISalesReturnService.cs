using System.Data;

namespace RetailSales.Interface.Sales
{
    public interface ISalesReturnService
    {
        DataTable GetAllSalesReturn(string strStatus);
    }
}
