using RetailSales.Interface.Sales;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Sales
{
    public class SalesReturnService : ISalesReturnService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesReturnService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllSalesReturn(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ID,DOC_NO,DOC_DATE,INVOICE_NO,INV_DATE,RETURN_TYPE,CUSTOMER,SAL_RETURN.IS_ACTIVE FROM  SAL_RETURN WHERE SAL_RETURN.IS_ACTIVE = 'Y' ORDER BY SAL_RETURN.ID ASC";
            }
            else
            {
                SvSql = "SELECT ID,DOC_NO,DOC_DATE,INVOICE_NO,INV_DATE,CUSTOMER,RETURN_TYPE,SAL_RETURN.IS_ACTIVE FROM  SAL_RETURN WHERE SAL_RETURN.IS_ACTIVE = 'N' ORDER BY SAL_RETURN.ID ASC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
