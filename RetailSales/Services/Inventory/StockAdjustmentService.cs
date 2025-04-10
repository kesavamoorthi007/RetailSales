using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Excel;
using RetailSales.Interface.Inventory;
using RetailSales.Models;
using RetailSales.Models.Inventory;

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
                SvSql = "SELECT STKADJBASICID,LOCATION.LOCATION_NAME,TYPE,DOCID,CONVERT(varchar,STKADJBASIC.DOCDATE,106) AS DOCDATE,STKADJBASIC.IS_ACTIVE FROM STKADJBASIC LEFT OUTER JOIN LOCATION ON LOCATION.ID=STKADJBASIC.LOCATION  WHERE STKADJBASIC.IS_ACTIVE = 'Y' ORDER BY STKADJBASIC.STKADJBASICID DESC";
            }
            else
            {
                SvSql = "SELECT STKADJBASICID,LOCATION.LOCATION_NAME,TYPE,DOCID,CONVERT(varchar,STKADJBASIC.DOCDATE,106) AS DOCDATE,STKADJBASIC.IS_ACTIVE FROM STKADJBASIC LEFT OUTER JOIN LOCATION ON LOCATION.ID=STKADJBASIC.LOCATION  WHERE STKADJBASIC.IS_ACTIVE = 'N' ORDER BY STKADJBASIC.STKADJBASICID DESC";

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

        public DataTable GetVariantDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT PRODUCT_DESCRIPTION,UOM.UOM_CODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM WHERE PRO_DETAIL.ID='" + ItemId + "'";
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

        public DataTable GetEditStockAdjustment(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LOCATION,TYPE,DOCID,CONVERT(varchar,STKADJBASIC.DOCDATE,106) AS DOCDATE,REASON FROM STKADJBASIC WHERE STKADJBASIC.STKADJBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditStockAdjustmentItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT STKADJBASICID,STKADJDETAILID,PRODUCT_NAME,PRODUCT,VARIANT,UOM,STOCKQTY,QTY,RATE,AMOUNT FROM STKADJDETAIL  WHERE STKADJDETAIL.STKADJBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetStockAdjustment(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT LOCATION.LOCATION_NAME, TYPE, DOCID, CONVERT(varchar, STKADJBASIC.DOCDATE,106) AS DOCDATE, REASON FROM STKADJBASIC LEFT OUTER JOIN LOCATION ON LOCATION.ID=STKADJBASIC.LOCATION WHERE STKADJBASIC.STKADJBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetStockAdjustmentItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT STKADJBASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,STKADJDETAIL.UOM,STOCKQTY,QTY,AMOUNT,STKADJDETAIL.RATE FROM STKADJDETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=STKADJDETAIL.PRODUCT_NAME LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=STKADJDETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=STKADJDETAIL.VARIANT WHERE STKADJDETAIL.STKADJBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StockAdjustmentCRUD(StockAdjustment cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'SA' AND IS_ACTIVE = 'Y'");
                    string DocId = string.Format("{0}{1}{2}", "SA/", "24-25/", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SA' AND IS_ACTIVE ='Y'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.DocId = DocId;
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("StkadjBasicProc", objConn);
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
                    objCmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = cy.Location;
                    objCmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = cy.Type;
                    objCmd.Parameters.Add("@docid", SqlDbType.NVarChar).Value = cy.DocId;
                    objCmd.Parameters.AddWithValue("@docdate", cy.DocDate);
                    objCmd.Parameters.Add("@reason", SqlDbType.NVarChar).Value = cy.Reason;
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.StockAdjustmentList != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (StockAdjustmentItem cp in cy.StockAdjustmentList)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into STKADJDETAIL (STKADJBASICID,PRODUCT_NAME,PRODUCT,VARIANT,UOM,STOCKQTY,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','" + cp.StockQty + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                        if(cy.Type == "Addition")
                                        {
                                            double qty = Convert.ToDouble(cp.Qty);

                                            DataTable dt = datatrans.GetData("Select * from INVENTORY_ITEM  where ITEM_ID='" + cp.Item + "' and PRODUCT='" + cp.Product + "' AND VARIANT='" + cp.Variant + "' AND BALANCE_QTY!=0");
                                            if (dt.Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                {
                                                    double sqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                                    if (sqty >= qty || sqty <= qty)
                                                    {
                                                        double bqty = sqty + qty;

                                                        string Sql = string.Empty;
                                                        Sql = "Update INVENTORY_ITEM SET BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                        SqlCommand objCmdsz = new SqlCommand(Sql, objConn);
                                                        objCmdsz.ExecuteNonQuery();


                                                        Sql = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,DEST_UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR,CREATED_ON,CREATED_BY) VALUES ('" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','','" + cp.Rate + "','Stock Adjustment','Plus','" + cp.Qty + "','Stock Adjustment','" + cy.DocDate + "','2024-2025','','') ";
                                                        SqlCommand objCmdsz1 = new SqlCommand(Sql, objConn);
                                                        objCmdsz1.ExecuteNonQuery();
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                
                                                
                                                string svSQL1 = "Insert into INVENTORY_ITEM (DOC_ID,DOC_DATE,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,BALANCE_QTY,MONTH,LOCATION_ID,FINANCIAL_YEAR) VALUES ('" + cy.DocId + "','" + cy.DocDate + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','" + cp.Rate + "','" + cp.Qty + "','" + DateTime.Now.ToString("MMMM") + "','Godown','2024-2025')";
                                                SqlCommand objCmddtss = new SqlCommand(svSQL1, objConn);
                                                Object Pid1 = objCmddtss.ExecuteScalar();
                                                

                                                string svSQL2 = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR) VALUES ('" + Pid1 + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','" + cp.Rate + "','Stock Adjustment','Plus','" + cp.Qty + "','Stock Adjustment','" + cy.DocDate + "','2024-2025')";
                                                SqlCommand objCmddtsss = new SqlCommand(svSQL2, objConn);
                                                objCmddtsss.ExecuteNonQuery();
                                                
                                                
                                                
                                            }
                                        }
                                        if (cy.Type == "Deduction")
                                        {
                                            double qty = Convert.ToDouble(cp.Qty);

                                            DataTable dt = datatrans.GetData("Select * from INVENTORY_ITEM  where ITEM_ID='" + cp.Item + "' and PRODUCT='" + cp.Product + "' AND VARIANT='" + cp.Variant + "' AND BALANCE_QTY!=0");
                                            if (dt.Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dt.Rows.Count; i++)
                                                {
                                                    double sqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                                    if (sqty >= qty)
                                                    {
                                                        double bqty = sqty - qty;

                                                        string Sql = string.Empty;
                                                        Sql = "Update INVENTORY_ITEM SET BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                        SqlCommand objCmdsz = new SqlCommand(Sql, objConn);
                                                        objCmdsz.ExecuteNonQuery();


                                                        Sql = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,DEST_UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR,CREATED_ON,CREATED_BY) VALUES ('" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','','" + cp.Rate + "','Stock Adjustment','Minus','" + cp.Qty + "','Stock Adjustment','" + cy.DocDate + "','2024-2025','','') ";
                                                        SqlCommand objCmdsz1 = new SqlCommand(Sql, objConn);
                                                        objCmdsz1.ExecuteNonQuery();
                                                    }


                                                }
                                            }
                                            else
                                            {
                                                
                                                
                                                string svSQL1 = "Insert into INVENTORY_ITEM (DOC_ID,DOC_DATE,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,BALANCE_QTY,MONTH,LOCATION_ID,FINANCIAL_YEAR) VALUES ('" + cy.DocId + "','" + cy.DocDate + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','" + cp.Rate + "','" + cp.Qty + "','" + DateTime.Now.ToString("MMMM") + "','Godown','2024-2025')";
                                                SqlCommand objCmddtss = new SqlCommand(svSQL1, objConn);
                                                Object Pid1 = objCmddtss.ExecuteScalar();
                                                
                                                

                                                string svSQL2 = "Insert into INVENTORY_ITEM_TRANS (INV_ITEM_ID,TRANS_ID,GRN_ID,ITEM_ID,PRODUCT,VARIANT,UOM,UNIT_COST,TRANS_TYPE,TRANS_IMPACT,TRANS_QTY,TRANS_NOTES,TRANS_DATE,FINANCIAL_YEAR) VALUES ('" + Pid1 + "','" + Pid + "','" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','" + cp.Rate + "','Stock Adjustment','Minus','" + cp.Qty + "','Stock Adjustment','" + cy.DocDate + "','2024-2025')";
                                                SqlCommand objCmddtsss = new SqlCommand(svSQL2, objConn);
                                                objCmddtsss.ExecuteNonQuery();
                                                
                                                
                                                
                                                
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete STKADJDETAIL WHERE STKADJBASICID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (StockAdjustmentItem cp in cy.StockAdjustmentList)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into STKADJDETAIL (STKADJBASICID,PRODUCT_NAME,PRODUCT,VARIANT,UOM,STOCKQTY,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Product + "','" + cp.Variant + "','" + cp.Unit + "','" + cp.StockQty + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
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
                    svSQL = "UPDATE STKADJBASIC SET IS_ACTIVE ='N' WHERE STKADJBASICID='" + id + "'";
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
                    svSQL = "UPDATE STKADJBASIC SET IS_ACTIVE = 'Y' WHERE STKADJBASICID='" + id + "'";
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
