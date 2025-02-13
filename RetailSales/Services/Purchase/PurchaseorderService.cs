﻿using DocumentFormat.OpenXml.Office.Word;
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
                SvSql = "SELECT POBASICID,PONO,CONVERT(varchar, POBASIC.PODATE, 106) AS PODATE,SUPPLIER.SUPPLIER_NAME,REF_NO,STATUS,POBASIC.IS_ACTIVE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME  WHERE POBASIC.IS_ACTIVE = 'Y' ORDER BY POBASIC.POBASICID DESC";
            }
            else
            {
                SvSql = "SELECT POBASICID,PONO,CONVERT(varchar, POBASIC.PODATE, 106) AS PODATE,SUPPLIER.SUPPLIER_NAME,REF_NO,STATUS,POBASIC.IS_ACTIVE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME  WHERE POBASIC.IS_ACTIVE = 'N' ORDER BY POBASIC.POBASICID DESC";

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
            SvSql = "    SELECT PRODUCT_DESCRIPTION,UOM.UOM_CODE,HSNMAST.HSCODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM LEFT OUTER JOIN HSNMAST ON HSNMAST.HSNMASTID=PRO_DETAIL.HSN_CODE\r\n WHERE PRO_DETAIL.ID='" + ItemId + "'";
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
        public DataTable GetPurchasOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PONO,CONVERT(varchar, POBASIC.PODATE, 106) AS PODATE,SUPPLIER.SUPPLIER_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,REF_NO,CONVERT(varchar, POBASIC.REF_DATE, 106) AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,CONVERT(varchar, POBASIC.LR_DATE, 106) AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPurchasOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PONO,CONVERT(varchar, POBASIC.PODATE, 106) AS PODATE,SUP_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,REF_NO,CONVERT(varchar, POBASIC.REF_DATE, 106) AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,CONVERT(varchar, POBASIC.LR_DATE, 106) AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchasOrderItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT POBASICID,PODETAILID,PRODUCT.PRODUCT_NAME,PRO_DETAIL.PRODUCT_VARIANT,HSN,PODETAIL.TARIFF,PODETAIL.UOM,QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT WHERE PODETAIL.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPurchasOrderItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT POBASICID,PODETAILID,ITEM,VARIANT,HSN,PODETAIL.TARIFF,PODETAIL.UOM,QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL  WHERE PODETAIL.POBASICID='" + id + "'";
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

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'PO' AND IS_ACTIVE = 'Y'");
                    string po = string.Format("{0}{1}{2}", "PO/", "24-25/" , (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PO' AND IS_ACTIVE ='Y'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.po = po;
                }
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
                    objCmd.Parameters.AddWithValue("@podate", cy.Podate);
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

                        if (cy.PurchaseorderLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PurchaseorderItem cp in cy.PurchaseorderLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PODETAIL (POBASICID,ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
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
                                        svSQL = "Insert into PODETAIL (POBASICID,ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "')";
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

        public string OrderToGRN(Purchaseorder cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId("SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'GRN' AND IS_ACTIVE = 'Y'");
                string docno = string.Format("{0}{1}{2}", "GRN/", "24-25/", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='GRN' AND IS_ACTIVE ='Y'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    svSQL = "Insert into GRN_BASIC (GRN_NO,GRN_DATE,SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,FRIGHTCHARGE,CGST,SGST,IGST,ROUNT_OFF,POBASIC_ID,IS_ACTIVE) (Select '" + docno + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "',SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,FRIGHTCHARGE,CGST,SGST,IGST,ROUNT_OFF,'" + cy.ID + "','Y' from POBASIC where POBASICID='" + cy.ID + "')";
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

                string quotid = datatrans.GetDataString("Select GRN_BASIC_ID from GRN_BASIC Where POBASIC_ID=" + cy.ID + "");
                using (SqlConnection objConnT = new SqlConnection(_connectionString))

                {
                    string Sql = "Insert into GRN_DETAIL (GRN_BASIC_ID,ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT) (Select '" + quotid + "',ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL WHERE POBASICID=" + cy.ID + ")";
                    SqlCommand objCmds = new SqlCommand(Sql, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

                using (SqlConnection objConnE = new SqlConnection(_connectionString))
                {
                    string Sql = "UPDATE POBASIC SET STATUS='GRN Generated' where POBASICID='" + cy.ID + "'";
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
