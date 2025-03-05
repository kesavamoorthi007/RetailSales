using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Inventory;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class ProductdetailService : IProductdetailService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ProductdetailService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllProductDeatilsGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRODUCT_VARIANT,VARIANT_NICKNAME,UOM.UOM_CODE,RATE,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM WHERE PRO_DETAIL.IS_ACTIVE = 'Y' ORDER BY PRO_DETAIL.ID DESC";
            }
            else
            {
                SvSql = "SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRODUCT_VARIANT,VARIANT_NICKNAME,UOM.UOM_CODE,RATE,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY LEFT OUTER JOIN UOM ON UOM.ID=PRO_DETAIL.UOM WHERE PRO_DETAIL.IS_ACTIVE = 'N' ORDER BY PRO_DETAIL.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditProductdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,PRODUCT_CATEGORY,PRODUCT_VARIANT,VARIANT_NICKNAME,PRODUCT_DESCRIPTION,UOM,HSN_CODE,RATE,MIN_QTY FROM PRO_DETAIL WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditProductdetailTable(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM_CONVERT.PRO_ID,SRC_UOM,DEST_UOM,CONVRT_FACTOR FROM UOM_CONVERT WHERE UOM_CONVERT.PRO_ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ProductdetailCRUD(Productdetail cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("ProdetailProc", objConn);
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
                    objCmd.Parameters.Add("@productcategory", SqlDbType.NVarChar).Value = cy.Product;
                    objCmd.Parameters.Add("@productvariant", SqlDbType.NVarChar).Value = cy.Varint;
                    objCmd.Parameters.Add("@varintnickname", SqlDbType.NVarChar).Value = cy.Varintnic;
                    objCmd.Parameters.Add("@productdescription", SqlDbType.NVarChar).Value = cy.Productdescription;
                    objCmd.Parameters.Add("@hsncode ", SqlDbType.NVarChar).Value = cy.Hsncode;
                    objCmd.Parameters.Add("@uom", SqlDbType.NVarChar).Value = cy.Uom;
                    objCmd.Parameters.Add("@rate ", SqlDbType.NVarChar).Value = cy.Rate;
                    objCmd.Parameters.Add("@minqty ", SqlDbType.NVarChar).Value = cy.Minqty;
                   
                    //if (cy.ID == null)
                    //{
                    //    objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = "CreateBy";
                    //    objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    //}
                    //else
                    //{
                    //    objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = "UpdateBy";
                    //    objCmd.Parameters.Add("@updatedon", SqlDbType.Date).Value = DateTime.Now;
                    //}
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();

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
                            if(cy.ID == null)
                            {
                                foreach (ProductDetailTable cp in cy.ProductDetailTablelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into UOM_CONVERT (PRO_ID,SRC_UOM,DEST_UOM,CONVRT_FACTOR) VALUES ('" + proid + "','" + cp.Src + "','" + cp.Des + "','" + cp.CF + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete UOM_CONVERT WHERE PRO_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (ProductDetailTable cp in cy.ProductDetailTablelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into UOM_CONVERT (PRO_ID,SRC_UOM,DEST_UOM,CONVRT_FACTOR) VALUES ('" + proid + "','" + cp.Src + "','" + cp.Des + "','" + cp.CF + "')";
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


        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select PRODUCT_NAME,ID From PRODUCT";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetUom()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,UOM_CODE FROM UOM";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetHsn()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT HSNMASTID,HSCODE FROM HSNMAST";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProductdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRODUCT_VARIANT,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY WHERE PRO_DETAIL.ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetProductdetailTable(string id)
        {
            string SvSql = string.Empty;
            //SvSql = " SELECT UOM_CONVERT.ID,PRO_ID,UOM.UOM_CODE,CONVRT_FACTOR,UOM_CONVERT.IS_ACTIVE FROM UOM_CONVERT LEFT OUTER JOIN UOM ON UOM.ID=UOM_CONVERT.SRC_UOM WHERE UOM_CONVERT.ID='" + id + "'";
            SvSql = " SELECT UOM_CONVERT.PRO_ID,SRC_UOM,DEST_UOM,CONVRT_FACTOR,UOM_CONVERT.IS_ACTIVE FROM UOM_CONVERT WHERE UOM_CONVERT.PRO_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSUOM()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM.ID,UOM.UOM_CODE FROM UOM";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDUOM()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT UOM.ID,UOM.UOM_CODE FROM UOM";
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
            return "";

        }

        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE PRO_DETAIL SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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