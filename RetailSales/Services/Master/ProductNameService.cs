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
                SvSql = "SELECT PRO_NAME.ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,DESCRIPTION,PRO_NAME.IS_ACTIVE FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.IS_ACTIVE = 'Y' ORDER BY PRO_NAME.ID DESC";
            }
            else
            {
                SvSql = "SELECT PRO_NAME.ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,DESCRIPTION,PRO_NAME.IS_ACTIVE FROM PRO_NAME LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_NAME.PRODUCT_CATEGORY WHERE PRO_NAME.IS_ACTIVE = 'N' ORDER BY PRO_NAME.ID DESC";
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
            SvSql = "Select PRODUCT_NAME,ID From PRODUCT";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditProductName(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PRO_NAME.ID,PRODUCT_CATEGORY,PROD_NAME,DESCRIPTION FROM PRO_NAME WHERE ID = '" + id + "' ";
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

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.ID == null)
                    {
                        svSQL = "Insert into PRO_NAME (PRODUCT_CATEGORY,PROD_NAME,DESCRIPTION) VALUES ('" + cy.Category + "','" + cy.ProName + "','" + cy.Description + "')";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                        //StatementType = "Insert";
                        //objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        svSQL = "Update PRO_NAME set PRODUCT_CATEGORY = '" + cy.Category + "',PROD_NAME = '" + cy.ProName + "',DESCRIPTION = '" + cy.Description + "' WHERE PRO_NAME.ID ='" + cy.ID + "'";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                        //StatementType = "Update";
                        //objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = cy.ID;
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
                    svSQL = "UPDATE PRO_NAME SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE PRO_NAME SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
