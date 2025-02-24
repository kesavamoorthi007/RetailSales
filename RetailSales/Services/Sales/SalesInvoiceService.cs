using Dapper;
using Irony.Parsing.Construction;
using RetailSales.Interface.Master;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Sales
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesInvoiceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
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

        public DataTable GetVarientDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM.UOM_CODE,HSNMAST.HSCODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM LEFT OUTER JOIN HSNMAST ON HSNMAST.HSNMASTID=PRO_DETAIL.HSN_CODE WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE_NAME,ID from STATE";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCity(string cityid)
        {
            string SvSql = string.Empty;
            SvSql = "select CITY_NAME,ID from CITY WHERE STATE_ID = '" + cityid + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetUOM()
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT UOM_CODE,ID FROM UOM";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllSalesInvoice(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SALES_INV.ID,SALES_INV.INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,SALES_INV.CUSTOMER,SALES_INV.ADDRESS,SALES_INV.TOTAL_AMOUNT,SALES_INV.IS_ACTIVE,SALES_INV.STATUS FROM SALES_INV WHERE SALES_INV.IS_ACTIVE = 'Y' ORDER BY SALES_INV.ID DESC";
            }
            else
            {
                SvSql = "SELECT SALES_INV.ID,SALES_INV.INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,SALES_INV.CUSTOMER,SALES_INV.ADDRESS,SALES_INV.TOTAL_AMOUNT,SALES_INV.IS_ACTIVE,SALES_INV.STATUS FROM SALES_INV WHERE SALES_INV.IS_ACTIVE = 'N' ORDER BY SALES_INV.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSalesInvoice(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,CUSTOMER,ADDRESS,MOBILE,REMARKS,DISCOUNT,TOTAL_AMOUNT from SALES_INV   where SALES_INV.ID =" + id + "";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesInvoiceItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT ID,INV_ID,ITEM,VARIENT,UOM,BIN_NO,QTY,RATE,AMOUNT,DISCOUNT,TOTAL FROM SAL_INV_DEATILS WHERE SAL_INV_DEATILS.INV_ID =" + id + "";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string InvoicetoReturn(string SalesId)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
               
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    svSQL = "Insert into SAL_RETURN (INVOICE_NO,INV_DATE,SAL_INVOICE_NO,CUSTOMER,DOC_NO,DOC_DATE,ADDRESS,MOBILE,REMARKS,DISCOUNT,TOTAL,TOTAL_AMOUNT,IS_ACTIVE) (Select INVOICE_NO,INV_DATE,'" + SalesId + "',CUSTOMER,'1','" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ,ADDRESS,MOBILE,REMARKS,DISCOUNT,TOTAL,TOTAL_AMOUNT,'Y' from SALES_INV where SALES_INV.ID='" + SalesId + "')";
                    SqlCommand objCmd = new SqlCommand(svSQL, objConn);
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }

                using (SqlConnection objConnE = new SqlConnection(_connectionString))
                {
                    string Sql = "UPDATE SALES_INV SET STATUS='Generated' where SALES_INV.ID='" + SalesId + "'";
                    SqlCommand objCmds = new SqlCommand(Sql, objConnE);
                    objConnE.Open();
                    objCmds.ExecuteNonQuery();
                    objConnE.Close();
                }

            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
       
        public async Task<IEnumerable<ExinvBasicItem>> GetBasicItem(string id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<ExinvBasicItem>("SELECT ID, INVOICE_NO, INV_DATE, CUSTOMER, ADDRESS, MOBILE, REMARKS, DISCOUNT, TOTAL, TOTAL_AMOUNT, IS_ACTIVE, STATUS FROM  SALES_INV  WHERE ID='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<ExinvDetailitem>> GetExinvItemDetail(string id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<ExinvDetailitem>(" SELECT ID, INV_ID, ITEM, VARIENT, UOM, BIN_NO, QTY, RATE, AMOUNT, DISCOUNT, TOTAL FROM SAL_INV_DEATILS where  INV_ID='" + id + "'", commandType: CommandType.Text);
            }
        }
    }
}
