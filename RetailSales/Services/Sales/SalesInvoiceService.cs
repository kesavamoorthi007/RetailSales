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
    }
}
