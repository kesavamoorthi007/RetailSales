using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class CCategoryService : ICCategoryService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CCategoryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetEditCCategoryDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,CUSTOMER_CATEGORY,DESCRIPTION FROM CUSTOMER_CATEGORY_MASTER WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllCCategoryGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT CUSTOMER_CATEGORY_MASTER.ID,CUSTOMER_CATEGORY,DESCRIPTION,CUSTOMER_CATEGORY_MASTER.STATUS FROM CUSTOMER_CATEGORY_MASTER WHERE CUSTOMER_CATEGORY_MASTER.STATUS = 'Y' ORDER BY CUSTOMER_CATEGORY_MASTER.ID DESC";
            }
            else
            {
                SvSql = "SELECT CUSTOMER_CATEGORY_MASTER.ID,CUSTOMER_CATEGORY,DESCRIPTION,CUSTOMER_CATEGORY_MASTER.STATUS FROM CUSTOMER_CATEGORY_MASTER WHERE CUSTOMER_CATEGORY_MASTER.STATUS = 'N' ORDER BY CUSTOMER_CATEGORY_MASTER.ID DESC";

            }
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
                    svSQL = "UPDATE CUSTOMER_CATEGORY_MASTER SET STATUS ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE CUSTOMER_CATEGORY_MASTER SET STATUS = 'Y' WHERE ID='" + id + "'";
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
        public string CCategoryCRUD(CCategory cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(CUSTOMER_CATEGORY) as cnt FROM CUSTOMER_CATEGORY_MASTER WHERE CUSTOMER_CATEGORY = LTRIM(RTRIM('" + cy.Category + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Customer Category Already Exist";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("CategoryProc", objConn);
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
                    objCmd.Parameters.Add("@categoryname", SqlDbType.NVarChar).Value = cy.Category;
                    objCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = cy.Description;
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
    }
}
