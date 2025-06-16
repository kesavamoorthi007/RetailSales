using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class RateService : IRateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RateService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }
       
        public DataTable GetAllRate(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                //SvSql = "SELECT PRO_NAME_BASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_NAME.IS_ACTIVE FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.IS_ACTIVE = 'Y' ORDER BY PRO_NAME.PRO_NAME_BASICID DESC";
                SvSql = "SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRODUCT_VARIANT,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PRO_DETAIL.PRODUCT_ID WHERE PRO_DETAIL.IS_ACTIVE = 'Y' ORDER BY PRO_DETAIL.ID DESC";
            }
            else
            {
                //SvSql = "SELECT PRO_NAME_BASICID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_NAME.IS_ACTIVE FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.IS_ACTIVE = 'N' ORDER BY PRO_NAME.PRO_NAME_BASICID DESC";
                SvSql = "SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRODUCT_VARIANT,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PRO_DETAIL.PRODUCT_ID WHERE PRO_DETAIL.IS_ACTIVE = 'N' ORDER BY PRO_DETAIL.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditRate(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATE_BASIC_ID,CONVERT(varchar, RATE_BASIC.DOC_DATE, 106) AS DOC_DATE,CONVERT(varchar, RATE_BASIC.VALID_FROM, 106) AS VALID_FROM,CONVERT(varchar, RATE_BASIC.VALID_TO, 106) AS VALID_TO FROM RATE_BASIC WHERE RATE_BASIC.RATE_BASIC_ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditRateDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,RATE_BASIC_ID,ITEM_NAME,VARIANT,UNIT,RATE FROM RATE_DETAIL WHERE RATE_DETAIL.RATE_BASIC_ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetproductDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRO_DETAIL.PRODUCT_VARIANT,PRO_DETAIL.UOM, PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY WHERE PRO_DETAIL.IS_ACTIVE = 'Y' ORDER BY PRO_DETAIL.ID DESC";
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

        public DataTable GetProdRate()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,RATE FROM PRO_DETAIL WHERE PRO_DETAIL.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetRateView(string id)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PRO_DETAIL.PRODUCT_ID WHERE PRO_DETAIL.ID='" + id + "'";
            //SvSql = " SELECT PRO_DETAIL.ID,PN1.PRODUCT_CATEGORY AS ProductCategory,PN1.PROD_NAME AS ProductName,PRO_DETAIL.PRODUCT_VARIANT,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRO_NAME PN1 ON PN1.PRO_NAME_BASICID = PRO_DETAIL.PRODUCT_ID LEFT OUTER JOIN PRO_NAME PN2 ON PN2.PRO_NAME_BASICID = PRO_DETAIL.PRODUCT_ID WHERE PRO_DETAIL.ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetRateViewTable(string id)
        {
            string SvSql = string.Empty;
            //SvSql = " SELECT UOM_CONVERT.ID,PRO_ID,UOM.UOM_CODE,CONVRT_FACTOR,UOM_CONVERT.IS_ACTIVE FROM UOM_CONVERT LEFT OUTER JOIN UOM ON UOM.ID=UOM_CONVERT.SRC_UOM WHERE UOM_CONVERT.ID='" + id + "'";
            SvSql = " SELECT UOM_CONVERT.SRC_UOM,UOM_CONVERT.PRO_ID,UOM.UOM_CODE,DEST_UOM,CF,RATE,PERCENTAGE,SALES_RATE FROM UOM_CONVERT LEFT OUTER JOIN UOM ON UOM.ID=UOM_CONVERT.SRC_UOM LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=UOM_CONVERT.PRO_ID WHERE UOM_CONVERT.PRO_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetOldRate(string id)
        {
            string SvSql = string.Empty;
            //SvSql = " SELECT UOM_CONVERT.ID,PRO_ID,UOM.UOM_CODE,CONVRT_FACTOR,UOM_CONVERT.IS_ACTIVE FROM UOM_CONVERT LEFT OUTER JOIN UOM ON UOM.ID=UOM_CONVERT.SRC_UOM WHERE UOM_CONVERT.ID='" + id + "'";
            SvSql = " SELECT R.FROM_RANGE,R.TO_RANGE,R.VARIENT,UOM.UOM_CODE,R.UOM,RATE,PERCENTAGE,SALES_RATE FROM RATE_MASTER R LEFT OUTER JOIN UOM ON UOM.ID=R.UOM LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=R.VARIENT WHERE R.VARIENT='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string CFCRUD(Rate cy, string proid)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    try
                    {

                        objConn.Open();
                        proid = cy.ID;
                        if (cy.RateListItemlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (RateListItem cp in cy.RateListItemlst)
                                {

                                    //if (cp.Isvalid == "Y")
                                    //{
                                    svSQL = "Insert into UOM_CONVERT (PRO_ID,SRC_UOM,DEST_UOM,CF,PERCENTAGE,SALES_RATE,CREATED_BY,CREATED_ON) VALUES ('" + proid + "','" + cp.SrcUomID + "','" + cp.DestUom + "','" + cp.CF + "','" + cp.Percentage + "','" + cp.SalesRate + "','" + userId + "','" + DateTime.Now + "')";
                                    SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                    //}

                                }
                            }
                            else
                            {
                                svSQL = "Delete UOM_CONVERT WHERE PRO_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (RateListItem cp in cy.RateListItemlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into UOM_CONVERT (PRO_ID,SRC_UOM,DEST_UOM,CF,PERCENTAGE,SALES_RATE) VALUES ('" + proid + "','" + cp.SrcUomID + "','" + cp.DestUom + "','" + cp.CF + "','" + cp.Percentage + "','" + cp.SalesRate + "')";
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

        public string RateCRUD(Rate cy, string proid)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    try
                    {

                        objConn.Open();
                        proid = cy.ID;
                        if (cy.RateListItemlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (RateListItem cp in cy.RateListItemlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into RATE_MASTER (VARIENT,FROM_RANGE,TO_RANGE,UOM,PUR_RATE,PERCENTAGE,SALES_RATE,CREATED_BY,CREATED_ON) VALUES ('" + proid + "','" + cp.FromRange + "','" + cp.ToRange + "','" + cp.DestUom + "','"+ cp.ProdRate +"','" + cp.Percentage + "','" + cp.SalesRate + "','" + userId + "','" + DateTime.Now + "')";
                                    SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete RATE_MASTER WHERE VARIENT='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (RateListItem cp in cy.RateListItemlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into RATE_MASTER (VARIENT,FROM_RANGE,TO_RANGE,UOM,PUR_RATE,PERCENTAGE,SALES_RATE,CREATED_BY,CREATED_ON) VALUES ('" + proid + "','" + cp.FromRange + "','" + cp.ToRange + "','" + cp.DestUom + "','" + cp.ProdRate + "','" + cp.Percentage + "','" + cp.SalesRate + "','" + userId + "','" + DateTime.Now + "')";
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

        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE RATE_BASIC SET IS_ACTIVE = 'Y' WHERE RATE_BASIC_ID='" + id + "'";
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE RATE_BASIC SET IS_ACTIVE ='N' WHERE RATE_BASIC_ID='" + id + "'";
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
