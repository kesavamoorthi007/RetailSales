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
                SvSql = "SELECT GRN_BASIC_ID,GRN_NO,CONVERT(varchar, GRN_BASIC.GRN_DATE, 106) AS GRN_DATE,SUP_NAME,NET,GRN_BASIC.IS_ACTIVE FROM GRN_BASIC   WHERE GRN_BASIC.IS_ACTIVE = 'Y' ORDER BY GRN_BASIC.GRN_BASIC_ID DESC";
            }
            else
            {
                SvSql = "SELECT GRN_BASIC_ID,GRN_NO,CONVERT(varchar, GRN_BASIC.GRN_DATE, 106) AS GRN_DATE,SUP_NAME,NET,GRN_BASIC.IS_ACTIVE FROM GRN_BASIC  WHERE GRN_BASIC.IS_ACTIVE = 'N' ORDER BY GRN_BASIC.GRN_BASIC_ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE GRN_BASIC SET IS_ACTIVE ='N' WHERE GRN_BASIC_ID='" + id + "'";
                    SqlCommand objCmds = new SqlCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }
    }
}
