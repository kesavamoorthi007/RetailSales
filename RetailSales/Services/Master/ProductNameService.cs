using System.Data;
using System.Data.SqlClient;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Services.Master
{
    public class ProductNameService : IProductNameService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductNameService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllProductNameGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT PRO_NAME_BASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,DESCRIPTION,PRO_NAME.IS_ACTIVE FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.IS_ACTIVE = 'Y' ORDER BY PRO_NAME.PRO_NAME_BASICID DESC";
            }
            else
            {
                SvSql = "SELECT PRO_NAME_BASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,DESCRIPTION,PRO_NAME.IS_ACTIVE FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.IS_ACTIVE = 'N' ORDER BY PRO_NAME.PRO_NAME_BASICID DESC";
            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select PRODUCT_NAME,ID,PRODUCT.IS_ACTIVE From PRODUCT WHERE PRODUCT.IS_ACTIVE = 'Y'";
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

        public DataTable GetUom()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,UOM_CODE FROM UOM WHERE UOM.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditProductName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRODUCT_CATEGORY,PROD_NAME,DESCRIPTION FROM PRO_NAME WHERE PRO_NAME.PRO_NAME_BASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditProductNameItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRODUCT_ID,ID,PRODUCT_VARIANT,UOM,HSN_CODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION,SHOP_BIN,GODOWN_BIN,LOCATION FROM PRO_DETAIL WHERE PRO_DETAIL.PRODUCT_ID = '" + id + "' AND PRO_DETAIL.IS_ACTIVE='Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProductName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRODUCT.PRODUCT_NAME,PROD_NAME,DESCRIPTION FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.PRO_NAME_BASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProductNameItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRODUCT_ID,PRO_DETAIL.ID,PRODUCT_VARIANT,UOM.UOM_CODE,HSNMAST.HSCODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION,LOCATION,B.BINID SHOP_BIN,B1.BINID GODOWN_BIN FROM PRO_DETAIL LEFT OUTER JOIN BINMASTER B ON B.ID=SHOP_BIN LEFT OUTER JOIN BINMASTER B1 on B1.ID=GODOWN_BIN LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM LEFT OUTER JOIN HSNMAST ON HSNMAST.HSNMASTID=PRO_DETAIL.HSN_CODE WHERE PRO_DETAIL.PRODUCT_ID= '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ProductNameCRUD(ProductName cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(PROD_NAME) as cnt FROM PRO_NAME WHERE PROD_NAME = LTRIM(RTRIM('" + cy.ProName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Product Name Already Exist";
                        return msg;
                    }
                }
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
                    objCmd.Parameters.Add("@ProdCategory", SqlDbType.NVarChar).Value = cy.Category;
                    objCmd.Parameters.Add("@ProdName", SqlDbType.NVarChar).Value = cy.ProName;
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

                        if (cy.ProductNameLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProductNameItem cp in cy.ProductNameLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {

                               //         svSQL = "if exists(Select ID from PRO_DETAIL where PRODUCT_ID = '" + Pid + "' and PRODUCT_VARIANT = '" + cp.Variant + @"')
                               //begin 
                               //    update VITAL_BOOKING set RESULT = '" + ap.B_Pulse + "' where BOOKING_BASIC_ID = '" + ap.AppID + "' and VITAL_NAME = '" + ap.B_Pulse_ID + @"' 
                               //end else begin  
                               //       Insert into PRO_DETAIL (PRODUCT_ID,PRODUCT_CATEGORY,PRODUCT_VARIANT,UOM,HSN_CODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION) VALUES ('" + Pid + "','" + cy.Category + "','" + cp.Variant + "','" + cp.Uom + "','" + cp.Hsn + "','" + cp.MinQty + "','" + cp.Rate + "','" + cp.ProdDesc + @"') SELECT SCOPE_IDENTITY()
                               //end";
                                       

                                        svSQL = "Insert into PRO_DETAIL (PRODUCT_ID,PRODUCT_CATEGORY,PRODUCT_VARIANT,UOM,HSN_CODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION,SHOP_BIN,GODOWN_BIN,LOCATION) VALUES ('" + Pid + "','" + cy.Category + "','" + cp.Variant + "','" + cp.Uom + "','" + cp.Hsn + "','" + cp.MinQty + "','" + cp.Rate + "','" + cp.ProdDesc + "','"+ cp.ShopBin +"','"+ cp.GodownBin +"','"+cp.Location+"') SELECT SCOPE_IDENTITY()";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        Object Cid = objCmds.ExecuteScalar();
                                        string proid = Cid.ToString();

                                        svSQL = @"INSERT INTO UOM_CONVERT (SRC_UOM, DEST_UOM, CF, IS_ACTIVE, PRO_ID)
                                                SELECT UOM, UOM, 1, 'Y', ID FROM PRO_DETAIL WHERE ID='"+ proid + "'";
                                        SqlCommand objCmds1 = new SqlCommand(svSQL, objConn);
                                        objCmds1.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                //svSQL = "Delete PRO_DETAIL WHERE PRODUCT_ID='" + cy.ID + "'";
                                //SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                //objCmdd.ExecuteNonQuery();
                                foreach (ProductNameItem cp in cy.ProductNameLst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        if(cp.ID  == null)
                                        {
                                            svSQL = "Insert into PRO_DETAIL (PRODUCT_ID,PRODUCT_CATEGORY,PRODUCT_VARIANT,UOM,HSN_CODE,MIN_QTY,RATE,PRODUCT_DESCRIPTION,SHOP_BIN,GODOWN_BIN,LOCATION) VALUES ('" + Pid + "','" + cy.Category + "','" + cp.Variant + "','" + cp.Uom + "','" + cp.Hsn + "','" + cp.MinQty + "','" + cp.Rate + "','" + cp.ProdDesc + "','"+ cp.ShopBin +"','"+ cp.GodownBin +"','"+cp.Location+"') SELECT SCOPE_IDENTITY()";
                                            SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                            Object Cid = objCmds.ExecuteScalar();
                                            string proid = Cid.ToString();

                                            svSQL = @"INSERT INTO UOM_CONVERT (SRC_UOM, DEST_UOM, CF, IS_ACTIVE, PRO_ID)
                                                    SELECT UOM, UOM, 1, 'Y', ID FROM PRO_DETAIL WHERE ID='" + proid + "'";
                                            SqlCommand objCmds1 = new SqlCommand(svSQL, objConn);
                                            objCmds1.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            svSQL = "UPDATE PRO_DETAIL SET PRODUCT_CATEGORY='" + cy.Category + "',PRODUCT_VARIANT='" + cp.Variant + "',UOM='" + cp.Uom + "',HSN_CODE='" + cp.Hsn + "',MIN_QTY='" + cp.MinQty + "',RATE='" + cp.Rate + "',PRODUCT_DESCRIPTION='" + cp.ProdDesc + "',SHOP_BIN='"+ cp.ShopBin + "',GODOWN_BIN='"+ cp.GodownBin + "',LOCATION='"+ cp.Location +"' WHERE ID='" + cp.ID + "'";
                                            SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                           objCmds.ExecuteNonQuery();
                                        }
                                      
                                       
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
        public string CFCRUD(Productdetail cy, string proid)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {

                    try
                    {

                        objConn.Open();
                        proid = cy.ID;
                        if (cy.ProductDetailTablelst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (ProductDetailTable cp in cy.ProductDetailTablelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into UOM_CONVERT (PRO_ID,SRC_UOM,DEST_UOM,CF) VALUES ('" + proid + "','" + cp.UOM + "','" + cp.Des + "','" + cp.CF + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }

                                }
                            }
                            else
                            {
                                //svSQL = "Delete UOM_CONVERT WHERE PRO_ID='" + cy.ID + "'";
                                //SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                //objCmdd.ExecuteNonQuery();
                                foreach (ProductDetailTable cp in cy.ProductDetailTablelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into UOM_CONVERT (PRO_ID,SRC_UOM,DEST_UOM,CF) VALUES ('" + proid + "','" + cp.UOM + "','" + cp.Des + "','" + cp.CF + "')";
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
                    svSQL = "UPDATE PRO_NAME SET IS_ACTIVE ='N' WHERE PRO_NAME_BASICID='" + id + "'";
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

        public string VariantDelete(string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE PRO_DETAIL SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
            return "Variant Deleted Successfully ! ";
        }

        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE PRO_NAME SET IS_ACTIVE = 'Y' WHERE PRO_NAME_BASICID='" + id + "'";
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
