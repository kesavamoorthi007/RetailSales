using DocumentFormat.OpenXml.Office2010.Word;
using RetailSales.Controllers.Purchase;
using RetailSales.Interface;
using RetailSales.Interface.Master;
using RetailSales.Interface.Purchase;

using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services
{
    public class StockTransferService : IStockTransferService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StockTransferService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }
        public DataTable GetAllStockTransferGRID(string strStatus)
        {
            string SvSql = string.Empty;

            if (strStatus == "Y" || strStatus == null)
            {
                strStatus = "Y";
            }
                SvSql = @"SELECT STB.ST_BASIC_ID, STB.STOCK_TRANSFER_ID,CONVERT(varchar,STB.STOCK_TRANSFER_DATE,106) AS STOCK_TRANSFER_DATE, STB.FROM_LOCATION, STB.TO_LOCATION, STB.IS_ACTIVE, FBM.BINID AS FBIN, TBM.BINID AS TOBIN,(
SELECT STRING_AGG(P.PRODUCT_NAME + ' - ' + PN.PROD_NAME, ', ') AS ProductList
FROM STOCK_TRAN_DETAIL SD
LEFT JOIN PRO_NAME PN ON SD.PRODUCT = PN.PRO_NAME_BASICID
LEFT JOIN PRODUCT P ON P.ID = SD.ITEM
WHERE SD.ST_BASIC_ID = STB.ST_BASIC_ID ) as PRODUCT  FROM  dbo.STOCK_TRAN_BASICS AS STB LEFT OUTER JOIN dbo.BINMASTER AS TBM ON STB.TO_BIN_ID = TBM.ID LEFT OUTER JOIN dbo.BINMASTER AS FBM ON STB.FROM_BIN_ID = FBM.ID WHERE STB.IS_ACTIVE='"+ strStatus + "' ORDER BY ST_BASIC_ID DESC";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {

            string SvSql = string.Empty;
            SvSql = "SELECT ID,PRODUCT_NAME,PRODUCT.IS_ACTIVE FROM PRODUCT WHERE PRODUCT.IS_ACTIVE = 'Y' ";
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
        //public DataTable GetStock()
        //{

        //    string SvSql = string.Empty;
        //    SvSql = "SELECT ID,BALANCE_QTY FROM INVENTORY_ITEM";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;

        //}
        public DataTable GetVarientDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "  SELECT UOM.UOM_CODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM  WHERE PRO_DETAIL.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetStockDetails(string ItemId,string floc)
        {
            string SvSql = string.Empty;
          //  SvSql = "select ID,BINID from BINMASTER where LOCID=(SELECT ID FROM LOCATION WHERE LOCATION_NAME='" + floc + "' )";
            SvSql = "SELECT INVENTORY_ITEM.BALANCE_QTY FROM INVENTORY_ITEM WHERE INVENTORY_ITEM.VARIANT='" + ItemId + "' AND LOCATION_ID='"+ floc + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetFBinDetails(string floc)
        {
            string SvSql = string.Empty;
            SvSql = "select ID,BINID from BINMASTER where LOCID=(SELECT ID FROM LOCATION WHERE LOCATION_NAME='" + floc + "' )";
            //SvSql = "SELECT INVENTORY_ITEM.BIN_ID FROM INVENTORY_ITEM WHERE INVENTORY_ITEM.VARIANT='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEM.ID,VARIANT,BIN_NO,UOM,RATE FROM ITEM  Where ITEM.ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetStockDetails(string ItemId)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT ITEM.ID,VARIANT,BIN_NO,UOM,RATE FROM ITEM  Where ITEM.ID='" + ItemId + "'";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        public DataTable GetFBin()
        {
            string SvSql = string.Empty;
            SvSql = "select BINID,ID from BINMASTER";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }

        public DataTable GetTBin()
        {
            string SvSql = string.Empty;
            SvSql = "select BINID,ID from BINMASTER";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetFLocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCATION_NAME,ID from LOCATION";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetTLocation()
        {
            string SvSql = string.Empty;
            SvSql = "select LOCATION_NAME,ID from LOCATION";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetStockTransfer(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT STB.STOCK_TRANSFER_ID, STB.STOCK_TRANSFER_DATE, STB.FROM_LOCATION, STB.TO_LOCATION FROM  dbo.STOCK_TRAN_BASICS AS STB WHERE STB.ST_BASIC_ID='" + id + "'";

            //SvSql = " SELECT STOCK_TRANSFER_ID,STOCK_TRANSFER_DATE,FROM_LOCATION,TO_LOCATION,FROM_BIN_ID,TO_BIN_ID FROM STOCK_TRAN_BASICS WHERE STOCK_TRAN_BASICS.ST_BASIC_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetStockTransferItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ST_BASIC_ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,UNIT,STOCK_TRAN_DETAIL.STOCK,FBM.BINID AS FBIN, TBM.BINID AS TOBIN,QUANTITY,AMOUNT,STOCK_TRAN_DETAIL.RATE FROM STOCK_TRAN_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=STOCK_TRAN_DETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=STOCK_TRAN_DETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=STOCK_TRAN_DETAIL.VARIANT LEFT OUTER JOIN dbo.BINMASTER AS TBM ON STOCK_TRAN_DETAIL.TO_BIN_ID = TBM.ID LEFT OUTER JOIN dbo.BINMASTER AS FBM ON STOCK_TRAN_DETAIL.FROM_BIN_ID = FBM.ID WHERE STOCK_TRAN_DETAIL.ST_BASIC_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StockTransferCRUD(StockTransfer cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                string fyear = datatrans.GetCurrentFYear(DateTime.Now);
                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'ST' AND IS_ACTIVE = 'Y'");
                    string Documentid = string.Format("{0}{1}{2}", "ST/", "24-25/", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='ST' AND IS_ACTIVE ='Y'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.Documentid = Documentid;
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    // Set the connection in the constructor
                    SqlCommand objCmd = new SqlCommand("StocktransferProc", objConn);
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

                    // Uncomment and set if needed:
                    // objCmd.Parameters.Add("@EmpId", SqlDbType.NVarChar).Value = cy.EmpId;
                    objCmd.Parameters.Add("@stocktransferid", SqlDbType.VarChar).Value = cy.Documentid;
                    objCmd.Parameters.Add("@stocktransferdate", SqlDbType.Date).Value = cy.DocumentDate;
                    objCmd.Parameters.Add("@fromlocation", SqlDbType.VarChar).Value = cy.Flocation;
                    objCmd.Parameters.Add("@tolocation", SqlDbType.VarChar).Value = cy.Tlocation;
                    //objCmd.Parameters.Add("@frombinid", SqlDbType.VarChar).Value = cy.FBin;
                    //objCmd.Parameters.Add("@tobinid", SqlDbType.VarChar).Value = cy.TBin;
                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = userId;
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = userId;
                        objCmd.Parameters.Add("@updatedon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;

                    try
                    {
                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.StockTransferItemLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (StockTransferItem cp in cy.StockTransferItemLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into STOCK_TRAN_DETAIL (ST_BASIC_ID,ITEM,PRODUCT,VARIANT,UNIT,STOCK,FROM_BIN_ID,TO_BIN_ID,QUANTITY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Stock + "','" + cp.FBin + "','" + cp.TBin + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                        double qty = Convert.ToDouble(cp.Qty);

                                        DataTable dt = datatrans.GetData("Select * from INVENTORY_ITEM  where ITEM_ID='" + cp.Item + "' and PRODUCT='" + cp.Product + "' AND VARIANT='" + cp.Varient + "' AND BALANCE_QTY!=0 AND LOCATION_ID='"+ cy.Flocation + "'");
                                        if (dt.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                double sqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                                if (sqty >= qty)
                                                {
                                                    double bqty = sqty - qty;

                                                    string Sql1 = "Update INVENTORY_ITEM SET BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                    SqlCommand objCmdsz = new SqlCommand(Sql1, objConn);
                                                    objCmdsz.ExecuteNonQuery();

                                                    string Sql2 = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,DEST_UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR,CREATED_ON,CREATED_BY) VALUES ('" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Unit + "','','" + cp.Rate + "','Stock Transfer','Minus','" + cp.Qty + "','Stock Transfer','" + cy.DocumentDate + "','" + fyear + "','" + DateTime.Now + "','" + userId + "') ";
                                                    SqlCommand objCmdsz1 = new SqlCommand(Sql2, objConn);
                                                    objCmdsz1.ExecuteNonQuery();

                                                    string Sql3 = "INSERT INTO INVENTORY_ITEM (DOC_ID,DOC_DATE,ITEM_ID,PRODUCT,VARIANT,REC_GOOD_QTY,UOM,BALANCE_QTY,IS_LOCKED,FINANCIAL_YEAR,WASTAGE,LOCATION_ID,INV_ITEM_STATUS,UNIT_COST,MONTH) VALUES ('" + cy.Documentid + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "',5,'" + cp.Unit + "','" + cp.Qty + "','N','2024-2025','0','"+ cy.Tlocation +"','','" + cp.Rate + "','" + DateTime.Now.ToString("MMMM") + "') SELECT SCOPE_IDENTITY()";
                                                    SqlCommand objCmdsz2 = new SqlCommand(Sql3, objConn);
                                                    Object Pid1 = objCmdsz2.ExecuteScalar();

                                                    string Sql4 = "INSERT INTO INVENTORY_ITEM_TRANS (GRN_ID,INV_ITEM_ID,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR) VALUES ('" + Pid + "','" + Pid1 + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Rate + "','Stock Transfer','Plus','" + cp.Qty + "','Stock Transfer','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','2024-2025')";
                                                    SqlCommand objCmdsz3 = new SqlCommand(Sql4, objConn);
                                                    objCmdsz3.ExecuteNonQuery();
                                                }


                                            }
                                        }
                                        else
                                        {
                                            string svSQL1 = "Insert into INVENTORY_ITEM (DOC_ID,DOC_DATE,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,BALANCE_QTY,MONTH,LOCATION_ID,FINANCIAL_YEAR) VALUES ('" + cy.Documentid + "','" + cy.DocumentDate + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Rate + "','" + cp.Qty + "','" + DateTime.Now.ToString("MMMM") + "','Store','" + fyear + "')";
                                            SqlCommand objCmddtss = new SqlCommand(svSQL1, objConn);
                                            Object Pid1 = objCmddtss.ExecuteScalar();
                                            
                                            
                                            string svSQL2 = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR) VALUES ('" + Pid1 + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Unit + "','','" + cp.Rate + "','Stock Transfer','Minus','" + cp.Qty + "','Stock Transfer','" + cy.DocumentDate + "','" + fyear + "')";
                                            SqlCommand objCmddtsss = new SqlCommand(svSQL2, objConn);
                                            objCmddtsss.ExecuteNonQuery();
                                            
                                            
                                        }
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete STOCK_TRAN_DETAIL WHERE ST_BASIC_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (StockTransferItem cp in cy.StockTransferItemLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into STOCK_TRAN_DETAIL (ST_BASIC_ID,ITEM,PRODUCT,VARIANT,UNIT,STOCK,QUANTITY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Stock + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
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

                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public DataTable GetEditStockTransferDetail1(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT STOCK_TRANSFER_ID,STOCK_TRANSFER_DATE,FROM_LOCATION,TO_LOCATION,FROM_BIN_ID,TO_BIN_ID,BROWSE_ORDER,IS_ACTIVE FROM Stocktransfer WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditStockTransfer(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ITEM,VARIANT,UNIT,STOCK,QUANTITY,RATE,AMOUNT,IS_ACTIVE ,REPORT_TO FROM Stocktransfer WHERE ID = '" + id + "' ";
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
                    svSQL = "UPDATE USER_REGIST SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
