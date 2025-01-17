using RetailSales.Controllers.Purchase;
using RetailSales.Interface.Master;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Purchase
{
    public class PurchaseorderService : IPurchaseorderService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseorderService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllListPurchaseorder(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SALES_INV.ID,SALES_INV.INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,SALES_INV.CUSTOMER,SALES_INV.ADDRESS,SALES_INV.TOTAL_AMOUNT,SALES_INV.IS_ACTIVE FROM SALES_INV WHERE SALES_INV.IS_ACTIVE = 'Y' ORDER BY SALES_INV.ID DESC";
            }
            else
            {
                SvSql = "SELECT SALES_INV.ID,SALES_INV.INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,SALES_INV.CUSTOMER,SALES_INV.ADDRESS,SALES_INV.TOTAL_AMOUNT,SALES_INV.IS_ACTIVE FROM SALES_INV WHERE SALES_INV.IS_ACTIVE = 'N' ORDER BY SALES_INV.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
