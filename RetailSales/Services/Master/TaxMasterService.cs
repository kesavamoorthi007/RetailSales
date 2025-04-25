using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class TaxMasterService : ITaxMasterService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public TaxMasterService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetEditTaxMaster(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,TAX_NAME,PERCENTAGE,TAX_DESC FROM TAXMASTER WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllTaxMaster(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT TAXMASTER.ID,TAX_NAME,PERCENTAGE,TAXMASTER.IS_ACTIVE FROM TAXMASTER WHERE TAXMASTER.IS_ACTIVE = 'Y' ORDER BY TAXMASTER.ID DESC";
            }
            else
            {
                SvSql = "SELECT TAXMASTER.ID,TAX_NAME,PERCENTAGE,TAXMASTER.IS_ACTIVE FROM TAXMASTER WHERE TAXMASTER.IS_ACTIVE = 'N' ORDER BY TAXMASTER.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string TaxMasterCRUD(TaxMaster cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                string Tax = cy.TaxName + " " + cy.Percentage + "%";
                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(TAX_NAME) as cnt FROM TAXMASTER WHERE TAX_NAME = LTRIM(RTRIM('" + cy.TaxName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Tax Name Already Exist";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.ID == null)
                    {
                        svSQL = "Insert into TAXMASTER (TAX_NAME,PERCENTAGE,TAX_DESC,CREATED_ON) VALUES ('" + cy.TaxName + "','" + cy.Percentage + "','" + cy.Taxdescription + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                        //StatementType = "Insert";
                        //objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else

                    {
                        svSQL = "Update TAXMASTER set TAX_NAME = '" + cy.TaxName + "',PERCENTAGE = '" + cy.Percentage + "',TAX_DESC = '" + cy.Taxdescription + "',UPDATED_ON ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE TAXMASTER.ID ='" + cy.ID + "'";
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
                    svSQL = "UPDATE TAXMASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE TAXMASTER SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
