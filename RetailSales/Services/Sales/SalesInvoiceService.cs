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
            SvSql = "Select INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,CUSTOMER,ADDRESS,MOBILE,GROSS,TOTAL_AMOUNT,DISCOUNT,FRIGHT,ROUND_OFF,AMTINWORDS,REMARKS from SALES_INV   where SALES_INV.ID =" + id + "";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesInvoiceItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT ID,INV_ID,ITEM,VARIENT,UOM,BIN_NO,QTY,DEST_UOM,CF,CF_QTY,RATE,AMOUNT,DISCOUNT,TOTAL FROM SAL_INV_DEATILS WHERE SAL_INV_DEATILS.INV_ID =" + id + "";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditSalesInvoice(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,CUSTOMER,SALES_INV.ADDRESS,SALES_INV.COUNTRY,SALES_INV.STATE,SALES_INV.CITY,MOBILE,GROSS,DISCOUNT,FRIGHT,ROUND_OFF,TOTAL_AMOUNT,AMTINWORDS,REMARKS FROM SALES_INV WHERE SALES_INV.ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditSalesInvoiceItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT INV_ID,ID,ITEM,VARIENT,HSN,SAL_INV_DEATILS.UOM,QTY,DEST_UOM,CF,CF_QTY,SAL_INV_DEATILS.RATE,AMOUNT,DISCOUNT,TOTAL FROM SAL_INV_DEATILS  WHERE SAL_INV_DEATILS.INV_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SalesInvoiceCRUD(SalesInvoice cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'SI' AND IS_ACTIVE = 'Y'");
                    string InvoiceNo = string.Format("{0}{1}{2}", "SI/", "24-25/", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SI' AND IS_ACTIVE ='Y'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.InvoiceNo = InvoiceNo;
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("SalesInvBasicProc", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = cy.ID;
                    }
                    objCmd.Parameters.Add("@Rt", SqlDbType.NVarChar).Value = cy.InvoiceNo;
                    objCmd.Parameters.AddWithValue("@Ivdate", cy.InvoiceDate);
                    objCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = cy.Customer;
                    objCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = "India";
                    objCmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = cy.State;
                    objCmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = cy.Mobile;
                    objCmd.Parameters.Add("@Gross", SqlDbType.Float).Value = cy.Gross;
                    objCmd.Parameters.Add("@Discount", SqlDbType.Float).Value = cy.Disc;
                    objCmd.Parameters.Add("@Fright", SqlDbType.Float).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("@Round", SqlDbType.Float).Value = cy.Round;
                    objCmd.Parameters.Add("@Total", SqlDbType.Float).Value = cy.Net;
                    objCmd.Parameters.Add("@Amountinwords", SqlDbType.NVarChar).Value = cy.Amountinwords;
                    objCmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = cy.Remarks;
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.SalesInvoiceLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (SalesInvoiceItem cp in cy.SalesInvoiceLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into SAL_INV_DEATILS (INV_ID,ITEM,VARIENT,HSN,UOM,QTY,DEST_UOM,CF,CF_QTY,RATE,AMOUNT,DISCOUNT,TOTAL) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.DestUOM + "','" + cp.CF + "','" + cp.CfQty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.Discount + "','" + cp.Total + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete SAL_INV_DEATILS WHERE INV_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (SalesInvoiceItem cp in cy.SalesInvoiceLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into SAL_INV_DEATILS (INV_ID,ITEM,VARIENT,HSN,UOM,QTY,DEST_UOM,CF,CF_QTY,RATE,AMOUNT,DISCOUNT,TOTAL) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.DestUOM + "','" + cp.CF + "','" + cp.CfQty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.Discount + "','" + cp.Total + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }

                        }




                    }

                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
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
                return await db.QueryAsync<ExinvBasicItem>("SELECT ID, INVOICE_NO, INV_DATE, CUSTOMER, ADDRESS, MOBILE, GROSS, DISCOUNT, FRIGHT, ROUND_OFF, NET, AMTINWORDS, REMARKS, IS_ACTIVE, STATUS FROM  SALES_INV  WHERE ID='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<ExinvDetailitem>> GetExinvItemDetail(string id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<ExinvDetailitem>(" SELECT ID, INV_ID, ITEM, VARIENT, UOM, BIN_NO, QTY, RATE, AMOUNT, DISCOUNT, TOTAL FROM SAL_INV_DEATILS where  INV_ID='" + id + "'", commandType: CommandType.Text);
            }
        }

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE SALES_INV SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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

        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE SALES_INV SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
