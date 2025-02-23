using RetailSales.Interface.Purchase;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Purchase
{
    public class DirectPurchaseService : IDirectPurchaseService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DirectPurchaseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllListDirectPurchase(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT DPBASICID,NET,DOC_NO,CONVERT(varchar, DPBASIC.DOC_DATE, 106) AS DOC_DATE,SUPPLIER.SUPPLIER_NAME,REF_NO,DPBASIC.IS_ACTIVE FROM DPBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=DPBASIC.SUP_NAME  WHERE DPBASIC.IS_ACTIVE = 'Y' ORDER BY DPBASIC.DPBASICID DESC";
            }
            else
            {
                SvSql = "SELECT DPBASICID,NET,DOC_NO,CONVERT(varchar, DPBASIC.DOC_DATE, 106) AS DOC_DATE,SUPPLIER.SUPPLIER_NAME,REF_NO,DPBASIC.IS_ACTIVE FROM DPBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=DPBASIC.SUP_NAME  WHERE DPBASIC.IS_ACTIVE = 'N' ORDER BY DPBASIC.DPBASICID DESC";

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
            SvSql = "select SUPPLIER_NAME,ADDRESS,STATE,CITY,SUPPLIER.ID from SUPPLIER  WHERE SUPPLIER.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVarientDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "    SELECT UOM.UOM_CODE,HSNMAST.HSCODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM LEFT OUTER JOIN HSNMAST ON HSNMAST.HSNMASTID=PRO_DETAIL.HSN_CODE\r\n WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetHsn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select HSN_CODE,ID from PRO_DETAIL WHERE ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GethsnDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select HSNMASTID from HSNMAST where HSCODE='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetgstDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TAXMASTER.TAX_NAME from HSNROW LEFT OUTER JOIN TAXMASTER ON TAXMASTER.ID=HSNROW.TARIFFID where HSNCODEID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditDirectPurchase(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOC_NO,CONVERT(varchar, DPBASIC.DOC_DATE, 106) AS DOC_DATE,SUP_NAME,DPBASIC.ADDRESS,DPBASIC.COUNTRY,DPBASIC.STATE,DPBASIC.CITY,REF_NO,CONVERT(varchar, DPBASIC.REF_DATE, 106) AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,CONVERT(varchar, DPBASIC.LR_DATE, 106) AS LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,CGST,SGST,IGST,ROUNT_OFF,FRIGHTCHARGE FROM DPBASIC WHERE DPBASIC.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchase(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DOC_NO,CONVERT(varchar, DPBASIC.DOC_DATE, 106) AS DOC_DATE,SUPPLIER.SUPPLIER_NAME,DPBASIC.ADDRESS,DPBASIC.COUNTRY,DPBASIC.STATE,DPBASIC.CITY,REF_NO,CONVERT(varchar, DPBASIC.REF_DATE, 106) AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,CONVERT(varchar, DPBASIC.LR_DATE, 106) AS LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,CGST,SGST,IGST,ROUNT_OFF,FRIGHTCHARGE FROM DPBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=DPBASIC.SUP_NAME WHERE DPBASIC.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditDirectPurchaseItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DPBASICID,DPDETAILID,ITEM,VARIANT,HSN,DPDETAIL.TARIFF,DPDETAIL.UOM,QTY,DPDETAIL.RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM DPDETAIL  WHERE DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDirectPurchaseItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT DPBASICID,DPDETAILID,PRODUCT.PRODUCT_NAME,PRO_DETAIL.PRODUCT_VARIANT,HSN,DPDETAIL.TARIFF,DPDETAIL.UOM,QTY,DPDETAIL.RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM DPDETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=DPDETAIL.ITEM LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=DPDETAIL.VARIANT  WHERE DPDETAIL.DPBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string DirectPurchaseCRUD(DirectPurchase cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId("SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'DP' AND IS_ACTIVE = 'Y'");
                    string doc = string.Format("{0}{1}{2}", "DP/", "24-25/", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='DP' AND IS_ACTIVE ='Y'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.doc = doc;
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("DPBasicProc", objConn);
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
                    objCmd.Parameters.Add("@doc", SqlDbType.NVarChar).Value = cy.doc;
                    objCmd.Parameters.AddWithValue("@docdate", cy.DocDate);
                    //objCmd.Parameters.Add("@podate", SqlDbType.NVarChar).Value = cy.Podate;
                    objCmd.Parameters.Add("@Suppid", SqlDbType.Int).Value = cy.Suppid;
                    objCmd.Parameters.Add("@Supplieraddress", SqlDbType.NVarChar).Value = cy.Supplieraddress;
                    objCmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = "India";
                    objCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = cy.State;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@refno", SqlDbType.NVarChar).Value = cy.refno;
                    objCmd.Parameters.AddWithValue("@refdate", cy.refdate);
                    //objCmd.Parameters.Add("@refdate", SqlDbType.NVarChar).Value = cy.refdate;
                    objCmd.Parameters.Add("@amountinwords", SqlDbType.NVarChar).Value = cy.Amountinwords;
                    objCmd.Parameters.Add("@narration", SqlDbType.NVarChar).Value = cy.Narration;
                    objCmd.Parameters.Add("@trans", SqlDbType.NVarChar).Value = cy.drivername;
                    objCmd.Parameters.Add("@lrno", SqlDbType.NVarChar).Value = cy.LRno;
                    //objCmd.Parameters.Add("@lrdate", SqlDbType.NVarChar).Value = cy.LRdate;
                    objCmd.Parameters.AddWithValue("@lrdate", cy.LRdate);
                    objCmd.Parameters.Add("@place", SqlDbType.NVarChar).Value = cy.dispatchname;
                    objCmd.Parameters.Add("@gross", SqlDbType.NVarChar).Value = cy.Gross;
                    objCmd.Parameters.Add("@net", SqlDbType.NVarChar).Value = cy.Net;
                    objCmd.Parameters.Add("@dis", SqlDbType.NVarChar).Value = cy.Disc;
                    objCmd.Parameters.Add("@fright", SqlDbType.NVarChar).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("@cgst", SqlDbType.NVarChar).Value = cy.CGST;
                    objCmd.Parameters.Add("@sgst", SqlDbType.NVarChar).Value = cy.SGST;
                    objCmd.Parameters.Add("@igst", SqlDbType.NVarChar).Value = cy.IGST;
                    objCmd.Parameters.Add("@round", SqlDbType.NVarChar).Value = cy.Round;
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.DirectPurchaseLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DirectPurchaseItem cp in cy.DirectPurchaseLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into DPDETAIL (DPBASICID,ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete DPDETAIL WHERE DPBASICID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DirectPurchaseItem cp in cy.DirectPurchaseLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into DPDETAIL (DPBASICID,ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
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
                    svSQL = "UPDATE DPBASIC SET IS_ACTIVE ='N' WHERE DPBASICID='" + id + "'";
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
                    svSQL = "UPDATE DPBASIC SET IS_ACTIVE = 'Y' WHERE DPBASICID='" + id + "'";
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
