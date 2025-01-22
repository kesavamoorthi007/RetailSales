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
                SvSql = "SELECT ID,DOC_NO,DOC_DATE,COMPANY_NAME,SUPPLIER_NAME,DELIVERY_ADDRESS,SUPPLIER_LOCATION,SUPPLIER_ADDRESS,PUR_ORD.IS_ACTIVE FROM PUR_ORD WHERE PUR_ORD.IS_ACTIVE = 'Y' ORDER BY PUR_ORD.ID DESC";
            }
            else
            {
                SvSql = "SELECT ID,DOC_NO,DOC_DATE,COMPANY_NAME,SUPPLIER_NAME,DELIVERY_ADDRESS,SUPPLIER_LOCATION,SUPPLIER_ADDRESS,PUR_ORD.IS_ACTIVE FROM PUR_ORD WHERE PUR_ORD.IS_ACTIVE = 'N' ORDER BY PUR_ORD.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
