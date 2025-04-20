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
            SvSql = "SELECT PRODUCT.ID,PRODUCT.PRODUCT_NAME,PRODUCT.IS_ACTIVE FROM PRODUCT WHERE PRODUCT.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProduct(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PROD_NAME,PRO_NAME_BASICID From PRO_NAME WHERE PRO_NAME.PRODUCT_CATEGORY='" + id + "' AND PRO_NAME.IS_ACTIVE = 'Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVariant(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,PRODUCT_VARIANT FROM PRO_DETAIL WHERE PRO_DETAIL.PRODUCT_ID='" + id + "' AND PRO_DETAIL.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetVarientDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM.ID UOM_ID,UOM.UOM_CODE,HSNMAST.HSCODE,UOM_CONVERT.SALES_RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM LEFT OUTER JOIN HSNMAST ON HSNMAST.HSNMASTID=PRO_DETAIL.HSN_CODE LEFT OUTER JOIN UOM_CONVERT ON UOM_CONVERT.PRO_ID=PRO_DETAIL.ID WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetStockDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT INVENTORY_ITEM.BALANCE_QTY FROM INVENTORY_ITEM WHERE INVENTORY_ITEM.VARIANT='" + ItemId + "'";
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
                SvSql = "SELECT SALES_INV.SAL_INV_BASICID,SALES_INV.INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,SALES_INV.CUSTOMER,SALES_INV.ADDRESS,SALES_INV.NET,SALES_INV.IS_ACTIVE,SALES_INV.STATUS FROM SALES_INV WHERE SALES_INV.IS_ACTIVE = 'Y' ORDER BY SALES_INV.SAL_INV_BASICID DESC";
            }
            else
            {
                SvSql = "SELECT SALES_INV.SAL_INV_BASICID,SALES_INV.INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,SALES_INV.CUSTOMER,SALES_INV.ADDRESS,SALES_INV.NET,SALES_INV.IS_ACTIVE,SALES_INV.STATUS FROM SALES_INV WHERE SALES_INV.IS_ACTIVE = 'N' ORDER BY SALES_INV.SAL_INV_BASICID DESC";

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
            SvSql = "Select INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,CUSTOMER,ADDRESS,STATE.STATE_NAME,CITY.CITY_NAME,MOBILE,GROSS,DISCOUNT,FRIGHT,ROUND_OFF,NET,AMTINWORDS,NARRATION from SALES_INV LEFT OUTER JOIN STATE ON STATE.ID=SALES_INV.STATE LEFT OUTER JOIN CITY ON CITY.ID=SALES_INV.CITY where SALES_INV.SAL_INV_BASICID =" + id + "";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesInvoiceItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT SAL_INV_DETAILID,SAL_INV_BASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,SAL_INV_DEATILS.HSN_CODE,SAL_INV_DEATILS.UOM,BIN_NO,QTY,DEST_UOM,CF,CF_QTY,SAL_INV_DEATILS.RATE,AMOUNT,DISC_PER,DISCOUNT,TOTAL FROM SAL_INV_DEATILS LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=SAL_INV_DEATILS.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=SAL_INV_DEATILS.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=SAL_INV_DEATILS.VARIENT WHERE SAL_INV_DEATILS.SAL_INV_BASICID =" + id + "";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditSalesInvoice(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT INVOICE_NO,CONVERT(varchar, SALES_INV.INV_DATE, 106) AS INV_DATE,CUSTOMER,SALES_INV.ADDRESS,SALES_INV.COUNTRY,SALES_INV.STATE,SALES_INV.CITY,MOBILE,GROSS,DISCOUNT,FRIGHT,ROUND_OFF,NET,AMTINWORDS,NARRATION FROM SALES_INV WHERE SALES_INV.SAL_INV_BASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditSalesInvoiceItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SAL_INV_DETAILID,SAL_INV_BASICID,ITEM,PRODUCT,VARIENT,HSN_CODE,SAL_INV_DEATILS.UOM,QTY,DEST_UOM,CF,CF_QTY,SAL_INV_DEATILS.RATE,AMOUNT,DISCOUNT,TOTAL FROM SAL_INV_DEATILS  WHERE SAL_INV_DEATILS.SAL_INV_BASICID='" + id + "'";
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
                    objCmd.Parameters.Add("@InvNo", SqlDbType.NVarChar).Value = cy.InvoiceNo;
                    objCmd.Parameters.AddWithValue("@InvDate", cy.InvoiceDate);
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
                    objCmd.Parameters.Add("@Net", SqlDbType.Float).Value = cy.Net;
                    objCmd.Parameters.Add("@Amountinwords", SqlDbType.NVarChar).Value = cy.Amountinwords;
                    objCmd.Parameters.Add("@Narration", SqlDbType.NVarChar).Value = cy.Remarks;
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
                                        svSQL = "Insert into SAL_INV_DEATILS (SAL_INV_BASICID,ITEM,PRODUCT,VARIENT,HSN_CODE,UOM,QTY,DEST_UOM,CF,CF_QTY,RATE,AMOUNT,DISC_PER,DISCOUNT,TOTAL) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.DestUOM + "','" + cp.CF + "','" + cp.CfQty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.DiscPer + "','" + cp.Discount + "','" + cp.Total + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                        double qty = Convert.ToDouble(cp.Qty);

                                        DataTable dt = datatrans.GetData("Select * from INVENTORY_ITEM  where ITEM_ID='" + cp.Item + "' and PRODUCT='" + cp.Product + "' AND VARIANT='" + cp.Varient + "' AND BALANCE_QTY!=0");
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                double sqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                                if(sqty >= qty)
                                                {
                                                    double bqty = sqty - qty;

                                                    string Sql = string.Empty;
                                                    Sql = "Update INVENTORY_ITEM SET BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                    SqlCommand objCmdsz = new SqlCommand(Sql, objConn);
                                                    objCmdsz.ExecuteNonQuery();


                                                    Sql = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,DEST_UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR,CREATED_ON,CREATED_BY) VALUES ('" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.UOM + "','" + cp.DestUOM + "','" + cp.Rate + "','Sales Invoice','Minus','" + cp.Qty + "','Sales Invoice','" + cy.InvoiceDate + "','2024-2025','','') ";
                                                    SqlCommand objCmdsz1 = new SqlCommand(Sql, objConn);
                                                    objCmdsz1.ExecuteNonQuery();
                                                }
                                                

                                            }
                                        }
                                        else
                                        {
                                            string svSQL1 = "Insert into INVENTORY_ITEM (ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,BALANCE_QTY,MONTH,LOCATION_ID,FINANCIAL_YEAR) VALUES ('" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.UOM + "','" + cp.Rate + "','" + cp.Qty + "','" + DateTime.Now.ToString("MMMM") + "','Godown','2024-2025')";
                                            SqlCommand objCmddtss = new SqlCommand(svSQL1, objConn);
                                            Object Pid1 = objCmddtss.ExecuteScalar();
                                            
                                            
                                            string svSQL2 = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR) VALUES ('" + Pid1 + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.UOM + "','','" + cp.Rate + "','Sales Invoice','Minus','" + cp.Qty + "','Sales Invoice','" + cy.InvoiceDate + "','2024-2025')";
                                            SqlCommand objCmddtsss = new SqlCommand(svSQL2, objConn);
                                            objCmddtsss.ExecuteNonQuery();
                                            
                                            
                                        }
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete SAL_INV_DEATILS WHERE SAL_INV_BASICID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (SalesInvoiceItem cp in cy.SalesInvoiceLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into SAL_INV_DEATILS (SAL_INV_BASICID,ITEM,PRODUCT,VARIENT,HSN_CODE,UOM,QTY,DEST_UOM,CF,CF_QTY,RATE,AMOUNT,DISCOUNT,TOTAL) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.DestUOM + "','" + cp.CF + "','" + cp.CfQty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.Discount + "','" + cp.Total + "')";
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
                    svSQL = "Insert into SAL_RETURN (INVOICE_NO,INV_DATE,SAL_INVOICE_NO,CUSTOMER,DOC_NO,DOC_DATE,ADDRESS,MOBILE,REMARKS,DISCOUNT,TOTAL,TOTAL_AMOUNT,IS_ACTIVE) (Select INVOICE_NO,INV_DATE,'" + SalesId + "',CUSTOMER,'1','" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ,ADDRESS,MOBILE,NARRATION,DISCOUNT,TOTAL,NET,'Y' from SALES_INV where SALES_INV.SAL_INV_BASICID='" + SalesId + "')";
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
                    string Sql = "UPDATE SALES_INV SET STATUS='Generated' where SALES_INV.SAL_INV_BASICID='" + SalesId + "'";
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
                return await db.QueryAsync<ExinvBasicItem>("SELECT SAL_INV_BASICID, INVOICE_NO, INV_DATE, CUSTOMER, ADDRESS, MOBILE, GROSS, DISCOUNT, FRIGHT, ROUND_OFF, NET, AMTINWORDS, NARRATION, IS_ACTIVE, STATUS FROM  SALES_INV  WHERE SAL_INV_BASICID='" + id + "'", commandType: CommandType.Text);
            }
        }
        public async Task<IEnumerable<ExinvDetailitem>> GetExinvItemDetail(string id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<ExinvDetailitem>(" SELECT SAL_INV_DETAILID, SAL_INV_BASICID, ITEM, PRODUCT, VARIENT, HSN_CODE, UOM, BIN_NO, QTY, DEST_UOM, CF, CF_QTY, RATE, AMOUNT, DISCOUNT, TOTAL FROM SAL_INV_DEATILS where  SAL_INV_BASICID='" + id + "'", commandType: CommandType.Text);
            }
        }

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE SALES_INV SET IS_ACTIVE ='N' WHERE SAL_INV_BASICID='" + id + "'";
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
                    svSQL = "UPDATE SALES_INV SET IS_ACTIVE = 'Y' WHERE SAL_INV_BASICID='" + id + "'";
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
