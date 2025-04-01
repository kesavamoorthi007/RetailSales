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

        public DataTable GetGRN(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "SELECT PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUPPLIER.SUPPLIER_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,POBASIC.GST_NO,REF_NO,FORMAT(POBASIC.REF_DATE, 'MM/dd/yyyy') AS REF_DATE,POBASIC.GST_NO,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,FORMAT(POBASIC.LR_DATE, 'MM/dd/yyyy') AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME WHERE POBASIC.POBASICID='" + id + "'";
            SvSql = "SELECT GRN_NO,FORMAT(GRN_BASIC.GRN_DATE, 'MM/dd/yyyy') AS GRN_DATE,SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,FORMAT(GRN_BASIC.REF_DATE, 'MM/dd/yyyy') AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,FORMAT(GRN_BASIC.LR_DATE, 'MM/dd/yyyy') AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM GRN_BASIC WHERE GRN_BASIC.GRN_BASIC_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGRNItem(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "SELECT POBASICID,PODETAILID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,PODETAIL.HSN,PODETAIL.TARIFF,PODETAIL.UOM,DEST_UOM,CONVT_FACTOR,QTY,CF_QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.ID=PODETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT WHERE PODETAIL.POBASICID='" + id + "'";
            SvSql = "SELECT GRN_BASIC_ID,GRN_DETAIL_ID,ITEM,PRODUCT,VARIANT,HSN,TARIFF,UOM,QTY,RECIVED_QTY,ACCEPTED_QTY,REJECTED_QTY,EXCEED_QTY,SHORT_QTY,DEST_UOM,CF,CF_QTY,RATE,AMOUNT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM GRN_DETAIL WHERE GRN_DETAIL.GRN_BASIC_ID='" + id + "'";
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
