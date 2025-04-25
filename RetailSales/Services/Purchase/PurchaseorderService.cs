using DocumentFormat.OpenXml.Office.Word;
using RetailSales.Controllers.Purchase;
using RetailSales.Interface.Master;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Purchase
{
    public class PurchaseorderService : IPurchaseorderService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PurchaseorderService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }
        public DataTable GetUOM()
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT UOM_CODE,ID FROM UOM WHERE UOM.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllListPurchaseorder(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT POBASICID,PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUPPLIER.SUPPLIER_NAME,REF_NO,STATUS,POBASIC.IS_ACTIVE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME  WHERE POBASIC.IS_ACTIVE = 'Y' ORDER BY POBASIC.POBASICID DESC";
            }
            else
            {
                SvSql = "SELECT POBASICID,PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUPPLIER.SUPPLIER_NAME,REF_NO,STATUS,POBASIC.IS_ACTIVE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME  WHERE POBASIC.IS_ACTIVE = 'N' ORDER BY POBASIC.POBASICID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<PurchaseorderItem> GetAllPurchaseOrderItem(string id)
        {
            List<PurchaseorderItem> cmpList = new List<PurchaseorderItem>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "SELECT POBASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,PODETAIL.UOM,QTY  FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PODETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT where PODETAIL.POBASICID='" + id + "'";
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PurchaseorderItem cmp = new PurchaseorderItem
                        {
                            ID = rdr["POBASICID"].ToString(),
                            Item = rdr["PRODUCT_NAME"].ToString(),
                            Product = rdr["PROD_NAME"].ToString(),
                            Varient = rdr["PRODUCT_VARIANT"].ToString(),
                            UOM = rdr["UOM"].ToString(),
                            Qty = rdr["QTY"].ToString()
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRODUCT.ID,PRODUCT.PRODUCT_NAME,PRODUCT.IS_ACTIVE FROM PRODUCT WHERE PRODUCT.IS_ACTIVE = 'Y' ";
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
            //SvSql = "SELECT ID,CONCAT(PRODUCT_VARIANT, ' - ', HSCODE) AS Variant_HSN FROM PRO_DETAIL JOIN HSNMAST ON HSN_CODE = HSNMASTID WHERE PRO_DETAIL.PRODUCT_CATEGORY='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDUOM()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM.ID,UOM.UOM_CODE FROM UOM WHERE UOM.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSupplierDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select SUPPLIER_NAME,ADDRESS,STATE.STATE_NAME,CITY,GST_NO,SUPPLIER.ID from SUPPLIER LEFT OUTER JOIN STATE ON STATE.ID=SUPPLIER.STATE WHERE SUPPLIER.ID='" + ItemId + "' AND SUPPLIER.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVarientDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM.ID UOM_ID,UOM.UOM_CODE,HSNMAST.HSCODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM LEFT OUTER JOIN HSNMAST ON HSNMAST.HSNMASTID=PRO_DETAIL.HSN_CODE WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetHsn()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT HSNMASTID,HSCODE FROM HSNMAST WHERE HSNMAST.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetHsn(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select HSN_CODE,ID from PRO_DETAIL WHERE ID='" + id + "' AND PRO_DETAIL.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GethsnDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select HSNMASTID from HSNMAST where HSCODE='" + id + "' AND HSNMAST.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetgstDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select TAXMASTER.TAX_NAME from HSNROW LEFT OUTER JOIN TAXMASTER ON TAXMASTER.ID=HSNROW.TARIFFID where HSNCODEID='" + id + "' AND TAXMASTER.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchasOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUPPLIER.SUPPLIER_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,REF_NO,FORMAT(POBASIC.REF_DATE, 'MM/dd/yyyy') AS REF_DATE,POBASIC.GST_NO,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,FORMAT(POBASIC.LR_DATE, 'MM/dd/yyyy') AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPurchasOrder(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUP_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,REF_NO,FORMAT(POBASIC.REF_DATE, 'MM/dd/yyyy') AS REF_DATE,GST_NO,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,FORMAT(POBASIC.LR_DATE, 'MM/dd/yyyy') AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGeneratePdf(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUPPLIER.SUPPLIER_NAME,POBASIC.ADDRESS,POBASIC.GST_NO FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGeneratePdfItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,POBASICID,PODETAILID,PODETAIL.ITEM,PRODUCT.PRODUCT_NAME,PODETAIL.PRODUCT,PRO_NAME.PROD_NAME,PODETAIL.VARIANT,PRO_DETAIL.PRODUCT_VARIANT,QTY FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PODETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT WHERE PODETAIL.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPurchasOrderItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,POBASICID,PODETAILID,PODETAIL.ITEM,PRODUCT.PRODUCT_NAME,PODETAIL.PRODUCT,PRO_NAME.PROD_NAME,PODETAIL.VARIANT,PRO_DETAIL.PRODUCT_VARIANT,PODETAIL.HSN,PODETAIL.TARIFF,PODETAIL.UOM,DEST_UOM,CONVT_FACTOR,QTY,CF_QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PODETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT WHERE PODETAIL.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPurchasOrderItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT POBASICID,PODETAILID,ITEM,VARIANT,PRODUCT,HSN,PODETAIL.TARIFF,PODETAIL.UOM,DEST_UOM,CONVT_FACTOR,QTY,CF_QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL  WHERE PODETAIL.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE_NAME,ID from STATE WHERE STATE.IS_ACTIVE = 'Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCity(string cityid)
        {
            string SvSql = string.Empty;
            SvSql = "select CITY_NAME,ID from CITY WHERE STATE_ID = '" + cityid + "' AND CITY.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "select CATGRY_NAME,ID from SUPPL_CATGRY WHERE SUPPL_CATGRY.IS_ACTIVE = 'Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SupplierCRUD(string Category, string Suppid, string SupplierAdd, string Days, string GST, string State, String City, string Mobile, string Landline, string Email)
        {
            string id = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    svSQL = "Insert into SUPPLIER (SUPP_CAT,SUPPLIER_NAME,ADDRESS,CR_DAYS,GST_NO,STATE,CITY,MOBILE_NO,LANDLINE_NO,EMAIL_ID) VALUES ('" + Category + "','" + Suppid + "','" + SupplierAdd + "','" + Days + "','" + GST + "','" + State + "','" + City + "','" + Mobile + "','" + Landline + "','" + Email + "') SELECT SCOPE_IDENTITY()";

                    SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                    objConn.Open();
                    Object Cid = objCmds.ExecuteScalar();
                    objConn.Close();
                    id = Cid.ToString();
                }





            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }

        public DataTable GetProdCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select PRODUCT_NAME,ID,PRODUCT.IS_ACTIVE From PRODUCT WHERE PRODUCT.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ProductNameCRUD(Purchaseorder cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("ProNameProc", objConn);
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
                    objCmd.Parameters.Add("@ProdCategory", SqlDbType.NVarChar).Value = cy.ProdCat;
                    objCmd.Parameters.Add("@ProdName", SqlDbType.NVarChar).Value = cy.Product;
                    objCmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = cy.Description;
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

                                        svSQL = "Insert into PRO_DETAIL (PRODUCT_ID,PRODUCT_CATEGORY,PRODUCT_VARIANT,UOM,HSN_CODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION) VALUES ('" + Pid + "','" + cy.ProdCat + "','" + cp.Varient + "','" + cp.UOM + "','" + cp.Hsn + "','" + cp.MinQty + "','" + cp.Rate + "','" + cp.ProdDesc + "') SELECT SCOPE_IDENTITY()";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        Object Cid = objCmds.ExecuteScalar();
                                        string proid = Cid.ToString();

                                        svSQL = @"INSERT INTO UOM_CONVERT (SRC_UOM, DEST_UOM, CF, IS_ACTIVE, PRO_ID)
                                                SELECT UOM, UOM, 1, 'Y', ID FROM PRO_DETAIL WHERE ID='" + proid + "'";
                                        SqlCommand objCmds1 = new SqlCommand(svSQL, objConn);
                                        objCmds1.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete PRO_DETAIL WHERE PRODUCT_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PurchaseorderItem cp in cy.PurchaseorderLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PRO_DETAIL (PRODUCT_ID,PRODUCT_CATEGORY,PRODUCT_VARIANT,UOM,HSN_CODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION) VALUES ('" + Pid + "','" + cy.Category + "','" + cp.Varient + "','" + cp.UOM + "','" + cp.Hsn + "','" + cp.MinQty + "','" + cp.Rate + "','" + cp.ProdDesc + "') SELECT SCOPE_IDENTITY()";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        Object Cid = objCmds.ExecuteScalar();
                                        string proid = Cid.ToString();

                                        svSQL = @"INSERT INTO UOM_CONVERT (SRC_UOM, DEST_UOM, CF, IS_ACTIVE, PRO_ID)
                                                SELECT UOM, UOM, 1, 'Y', ID FROM PRO_DETAIL WHERE ID='" + proid + "'";
                                        SqlCommand objCmds1 = new SqlCommand(svSQL, objConn);
                                        objCmds1.ExecuteNonQuery();
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

        //public string ProductNameCRUD(string category, string product, string description, string varient, string uOM, string hsn, string minQty, string rate, string prodDesc)
        //{
        //    throw new NotImplementedException();
        //}

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
                    string po = string.Format("{0}{1}{2}", "PO/", "24-25/", (idc + 1).ToString());

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
                    //objCmd.Parameters.Add("@dis", SqlDbType.NVarChar).Value = cy.Disc;
                    objCmd.Parameters.Add("@fright", SqlDbType.NVarChar).Value = cy.Frieghtcharge;
                    objCmd.Parameters.Add("@cgst", SqlDbType.NVarChar).Value = cy.CGST;
                    objCmd.Parameters.Add("@sgst", SqlDbType.NVarChar).Value = cy.SGST;
                    objCmd.Parameters.Add("@igst", SqlDbType.NVarChar).Value = cy.IGST;
                    objCmd.Parameters.Add("@round", SqlDbType.NVarChar).Value = cy.Round;
                    objCmd.Parameters.Add("@gstno", SqlDbType.NVarChar).Value = cy.GST;
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
                                        svSQL = "Insert into PODETAIL (POBASICID,ITEM,PRODUCT,VARIANT,HSN,TARIFF,UOM,DEST_UOM,CONVT_FACTOR,QTY,CF_QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT,DISC_PER) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.UOM + "','" + cp.CF + "','" + cp.Qty + "','" + cp.CfQty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "','" + cp.DiscPer + "')";
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
                                        svSQL = "Insert into PODETAIL (POBASICID,ITEM,PRODUCT,VARIANT,HSN,TARIFF,UOM,DEST_UOM,CONVT_FACTOR,QTY,CF_QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT,DISC_PER) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.UOM + "','" + cp.CF + "','" + cp.Qty + "','" + cp.CfQty + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "','" + cp.DiscPer + "')";
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
                string StatementType = string.Empty; 
                string svSQL = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
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
                    string SvSql1 = "Insert into GRN_BASIC (GRN_NO,GRN_DATE,SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,FRIGHTCHARGE,CGST,SGST,IGST,ROUNT_OFF,POBASIC_ID,IS_ACTIVE) VALUES ('" + docno + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Suppid + "','" + cy.Supplieraddress + "','" + cy.Country + "','" + cy.State + "','" + cy.City + "','" + cy.refno + "','" + cy.refdate + "','" + cy.Amountinwords + "' ,'" + cy.Narration + "','" + cy.drivername + "','" + cy.LRno + "','" + cy.LRdate + "','" + cy.dispatchname + "','" + cy.Gross + "','" + cy.Net + "','" + cy.Disc + "','" + cy.Frieghtcharge + "','" + cy.CGST + "','" + cy.SGST + "','" + cy.IGST + "','" + cy.Round + "','" + cy.ID + "','Y') SELECT SCOPE_IDENTITY()";
                    SqlCommand objCmdsss = new SqlCommand(SvSql1, objConn);
                    objConn.Open();
                    Object Pid = objCmdsss.ExecuteScalar();

                    objConn.Close();

                    foreach (PurchaseorderItem cp in cy.PurchaseorderLst)
                    {
                        string SvSql2 = "Insert into GRN_DETAIL (GRN_BASIC_ID,ITEM,PRODUCT,VARIANT,HSN,TARIFF,UOM,QTY,RECIVED_QTY,RATE,AMOUNT,FRIGHT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT,DEST_UOM,CF,CF_QTY,ACCEPTED_QTY,REJECTED_QTY,EXCEED_QTY,SHORT_QTY) VALUES ('" + Pid + "','" + cp.Itemid + "','" + cp.Productid + "','" + cp.Varientid + "','" + cp.Hsn + "','" + cp.Tariff + "','" + cp.UOM + "','" + cp.Qty + "','" + cp.Recived + "','" + cp.Rate + "','" + cp.Amount + "','" + cp.FrigCharge + "','" + cp.DiscPer + "','" + cp.DiscAmount + "','" + cp.CGSTP + "','" + cp.SGSTP + "','" + cp.IGSTP + "','" + cp.CGST + "','" + cp.SGST + "','" + cp.IGST + "','" + cp.Total + "','" + cp.DestUOM + "','" + cp.CF + "','" + cp.CfQty + "','" + cp.Accepted + "','" + cp.Rejected + "','" + cp.exqty + "','" + cp.shortqty + "')";
                        SqlCommand objCmddts = new SqlCommand(SvSql2, objConn);
                        objConn.Open();
                        objCmddts.ExecuteNonQuery();
                        objConn.Close();

                        //string varient = datatrans.GetDataString("SELECT ID FROM PRO_DETAIL WHERE PRODUCT_VARIANT='" + cp.Varient + "'");
                        string svsql3 = "INSERT INTO INVENTORY_ITEM (DOC_ID,DOC_DATE,ITEM_ID,PRODUCT,VARIANT,REC_GOOD_QTY,UOM,BALANCE_QTY,IS_LOCKED,FINANCIAL_YEAR,WASTAGE,LOCATION_ID,INV_ITEM_STATUS,UNIT_COST,MONTH,CREATED_BY,CREATED_ON) VALUES ('" + docno + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.Itemid + "','" + cp.Productid + "','" + cp.Varientid + "',5,'" + cp.UOM + "','" + cp.Qty + "','N','2024-2025','0','Godown','','" + cp.Rate + "','" + DateTime.Now.ToString("MMMM") + "','" + userId + "','" + DateTime.Now + "') SELECT SCOPE_IDENTITY()";
                        SqlCommand objCmddtss = new SqlCommand(svsql3, objConn);
                        objConn.Open();
                        Object Pid1 = objCmddtss.ExecuteScalar();
                        objConn.Close();

                        string svsql4 = "INSERT INTO INVENTORY_ITEM_TRANS (GRN_ID,INV_ITEM_ID,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR,CREATED_BY,CREATED_ON) VALUES ('" + Pid + "','" + Pid1 + "','" + cp.Itemid + "','" + cp.Productid + "','" + cp.Varientid + "','" + cp.UOM + "','" + cp.Rate + "','GRN','Plus','" + cp.Qty + "','GRN','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','2024-2025','" + userId + "','" + DateTime.Now + "')";
                        SqlCommand objCmddtsss = new SqlCommand(svsql4, objConn);
                        objConn.Open();
                        objCmddtsss.ExecuteNonQuery();
                        objConn.Close();

                    }


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
