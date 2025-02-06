using DocumentFormat.OpenXml.Office.Word;
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
                SvSql = "SELECT POBASICID,PONO,PODATE,SUP_NAME,REF_NO,IS_ACTIVE FROM POBASIC  WHERE POBASIC.IS_ACTIVE = 'Y' ORDER BY POBASIC.POBASICID DESC";
            }
            else
            {
                SvSql = "SELECT POBASICID,PONO,PODATE,SUP_NAME,REF_NO,IS_ACTIVE FROM POBASIC  WHERE POBASIC.IS_ACTIVE = 'N' ORDER BY POBASIC.POBASICID DESC";

            }
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
        public DataTable GetSupplierDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select SUPPLIER_NAME,ADDRESS,STATE.STATE_NAME,CITY.CITY_NAME,SUPPLIER.ID from SUPPLIER  LEFT OUTER JOIN STATE ON STATE.ID=SUPPLIER.STATE LEFT OUTER JOIN CITY ON CITY.ID=SUPPLIER.CITY WHERE SUPPLIER.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVarientDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT PRODUCT_DESCRIPTION,UOM.UOM_CODE,HSN_CODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchasOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT POBASICID,PONO,PODATE,SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,REF_DATE,AMOUNT,AMTINWORDS,NARRATION,IS_ACTIVE FROM POBASIC WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchasOrderItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT POBASICID,ITEM,VARIANT,UOM,HSN,QTY,BIN_NO,RATE,AMOUNT FROM PODETAIL WHERE PODETAIL.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PurchaseorderCRUD(Purchaseorder cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                //if (cy.ID == null)
                //{

                //    svSQL = "SELECT Count(COMPANY_NAME) as cnt FROM COMPANY WHERE COMPANY_NAME = LTRIM(RTRIM('" + cy.CompanyName + "'))";
                //    if (datatrans.GetDataId(svSQL) > 0)
                //    {
                //        msg = "Company Name Already Existed";
                //        return msg;
                //    }
                //}
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("PoBasicProc", objConn);
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
                    objCmd.Parameters.Add("@po", SqlDbType.NVarChar).Value = cy.po;
                    objCmd.Parameters.Add("@podate", SqlDbType.NVarChar).Value = cy.Podate;
                    objCmd.Parameters.Add("@Suppid", SqlDbType.Int).Value = cy.Suppid;
                    objCmd.Parameters.Add("@Supplieraddress", SqlDbType.NVarChar).Value = cy.Supplieraddress;
                    objCmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = "India";
                    objCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = cy.State;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@refno", SqlDbType.NVarChar).Value = cy.refno;
                    objCmd.Parameters.Add("@refdate", SqlDbType.NVarChar).Value = cy.refdate;
                    objCmd.Parameters.Add("@total", SqlDbType.Float).Value = cy.Total;
                    objCmd.Parameters.Add("@amountinwords", SqlDbType.NVarChar).Value = cy.Amountinwords;
                    objCmd.Parameters.Add("@narration", SqlDbType.NVarChar).Value = cy.Narration;
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.PurchaseorderLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PurchaseorderItem cp in cy.PurchaseorderLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PODETAIL (POBASICID,ITEM,VARIANT,UOM,HSN,QTY,BIN_NO,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.UOM + "','" + cp.Hsn + "','" + cp.Qty + "','145','" + cp.Amount + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete PODETAIL WHERE POBASICID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PurchaseorderItem cp in cy.PurchaseorderLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PODETAIL (POBASICID,ITEM,VARIANT,UOM,HSN,QTY,BIN_NO,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.UOM + "','" + cp.Hsn + "','" + cp.Qty + "','145','" + cp.Amount + "')";
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE POBASIC SET IS_ACTIVE ='N' WHERE POBASICID='" + id + "'";
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
                    svSQL = "UPDATE POBASIC SET IS_ACTIVE = 'Y' WHERE POBASICID='" + id + "'";
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
