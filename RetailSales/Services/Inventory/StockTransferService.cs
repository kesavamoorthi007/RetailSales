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
        public StockTransferService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllStockTransferGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT STB.ST_BASIC_ID, STB.STOCK_TRANSFER_ID, STB.STOCK_TRANSFER_DATE, STB.FROM_LOCATION, STB.TO_LOCATION, STB.IS_ACTIVE, FBM.BINID AS FBIN, TBM.BINID AS TOBIN FROM  dbo.STOCK_TRAN_BASICS AS STB LEFT OUTER JOIN dbo.BINMASTER AS TBM ON STB.TO_BIN_ID = TBM.ID LEFT OUTER JOIN dbo.BINMASTER AS FBM ON STB.FROM_BIN_ID = FBM.ID ORDER BY ST_BASIC_ID DESC";

            }
            else
            {
                SvSql = "SELECT ST_BASIC_ID,STOCK_TRANSFER_ID,STOCK_TRANSFER_DATE,FROM_LOCATION,TO_LOCATION,FROM_BIN_ID,TO_BIN_ID,IS_ACTIVE   FROM STOCK_TRAN_BASICS ORDER BY ST_BASIC_ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable Item
        {
            get
            {
                string SvSql = string.Empty;
                SvSql = "SELECT ID,PRODUCT_NAME FROM PRODUCT";
                DataTable dtt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Fill(dtt);
                return dtt;
            }
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
            SvSql = "  SELECT UOM.UOM_CODE,RATE FROM PRO_DETAIL LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM WHERE PRO_DETAIL.ID='" + ItemId + "'";
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
        //public DataTable GetStockTransferItem(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT ST_BASIC_ID,ITEM,VARIANT,UNIT,STOCK,QUANTITY,RATE,AMOUNT FROM STOCK_TRAN_DETAIL WHERE STOCK_TRAN_DETAIL.ST_BASIC_ID='" + id + "'";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public string StockTransferCRUD(StockTransfer cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

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
                    objCmd.Parameters.Add("@frombinid", SqlDbType.VarChar).Value = cy.FBin;
                    objCmd.Parameters.Add("@tobinid", SqlDbType.VarChar).Value = cy.TBin;
                   
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
                                        svSQL = "Insert into STOCK_TRAN_DETAIL (ST_BASIC_ID,ITEM,VARIANT,UNIT,STOCK,QUANTITY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Stock + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
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
                                        svSQL = "Insert into STOCK_TRAN_DETAIL (ST_BASIC_ID,ITEM,VARIANT,UNIT,STOCK,QUANTITY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Item + "','" + cp.Varient + "','" + cp.Unit + "','" + cp.Stock + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amount + "')";
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
            SvSql = "SELECT STOCK_TRANSFER_ID,STOCK_TRANSFER_DATE,FROM_LOCATION,TO_LOCATION,FROM_BIN_ID,TO_BIN_ID,BROWSE_ORDER,ITEM,VARIANT,UNIT,STOCK,QUANTITY,RATE,AMOUNT,IS_ACTIVE ,REPORT_TO FROM Stocktransfer WHERE ID = '" + id + "' ";
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
        //public string RemoveChange(string tag, string id)
        //{

        //    try
        //    {
        //        string svSQL = string.Empty;
        //        using (SqlConnection objConnT = new SqlConnection(_connectionString))
        //        {
        //            svSQL = "UPDATE StockTransfer SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
        //            SqlCommand objCmds = new SqlCommand(svSQL, objConnT);
        //            objConnT.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConnT.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return "";

        //}



    }

}
