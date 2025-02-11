using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;
using RetailSales.Interface.Inventory;
using RetailSales.Models;

namespace RetailSales.Services.Inventory
{
    public class StockAdjustmentService : IStockAdjustmentService
    {

        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockAdjustmentService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllStockAdjustment(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT STKADJBASICID,LOCATION.LOCATION_NAME,TYPE,DOCID,DOCDATE,CONVERT(varchar, STKADJBASIC.DOCDATE, 106) AS DOCDATE,STKADJBASIC.IS_ACTIVE FROM STKADJBASIC LEFT OUTER JOIN LOCATION ON LOCATION.ID=STKADJBASIC.LOCATION  WHERE STKADJBASIC.IS_ACTIVE = 'Y' ORDER BY STKADJBASIC.STKADJBASICID DESC";
            }
            else
            {
                SvSql = "SELECT STKADJBASICID,LOCATION.LOCATION_NAME,TYPE,DOCID,DOCDATE,CONVERT(varchar, STKADJBASIC.DOCDATE, 106) AS DOCDATE,STKADJBASIC.IS_ACTIVE FROM STKADJBASIC LEFT OUTER JOIN LOCATION ON LOCATION.ID=STKADJBASIC.LOCATION  WHERE STKADJBASIC.IS_ACTIVE = 'N' ORDER BY STKADJBASIC.STKADJBASICID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCATION_NAME,ID from LOCATION";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRODUCT.ID,PRODUCT.PRODUCT_NAME FROM PRODUCT";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVariant(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,PRODUCT_VARIANT FROM PRO_DETAIL WHERE PRO_DETAIL.PRODUCT_CATEGORY='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVariantDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT PRODUCT_DESCRIPTION,UOM.UOM_CODE,HSN_CODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditStockAdjustmentItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT PRODUCT_DESCRIPTION,UOM.UOM_CODE,HSN_CODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM ;";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
       
    }
}
