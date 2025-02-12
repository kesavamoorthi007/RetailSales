using RetailSales.Interface.Purchase;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Purchase
{
    public class GRNService : IGRNService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public GRNService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllListGRN(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT GRN_BASIC_ID,GRN_NO,CONVERT(varchar, GRN_BASIC.GRN_DATE, 106) AS GRN_DATE,SUPPLIER.SUPPLIER_NAME,NET,GRN_BASIC.IS_ACTIVE FROM GRN_BASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=GRN_BASIC.SUP_NAME  WHERE GRN_BASIC.IS_ACTIVE = 'Y' ORDER BY GRN_BASIC.GRN_BASIC_ID DESC";
            }
            else
            {
                SvSql = "SELECT GRN_BASIC_ID,GRN_NO,CONVERT(varchar, GRN_BASIC.GRN_DATE, 106) AS GRN_DATE,SUPPLIER.SUPPLIER_NAME,NET,GRN_BASIC.IS_ACTIVE FROM GRN_BASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=GRN_BASIC.SUP_NAME  WHERE GRN_BASIC.IS_ACTIVE = 'N' ORDER BY GRN_BASIC.GRN_BASIC_ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
