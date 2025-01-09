using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class CompanyService : ICompanyService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CompanyService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllCompanyGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT COMPANY.ID,COMPANY_CODE,COMPANY_NAME,CITY,STATE,COUNTRY,COMPANY.IS_ACTIVE FROM COMPANY WHERE COMPANY.IS_ACTIVE = 'Y' ORDER BY COMPANY.ID DESC";
            }
            else
            {
                SvSql = "SELECT COMPANY.ID,COMPANY_CODE,COMPANY_NAME,CITY,STATE,COUNTRY,COMPANY.IS_ACTIVE FROM COMPANY WHERE COMPANY.IS_ACTIVE = 'N' ORDER BY COMPANY.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditCompanyDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT COMPANY_CODE,COMPANY_NAME,ADDRESS1,CITY,STATE,COUNTRY,TELEPHONE1,TELEPHONE2,FAX,EMAIL,CST FROM COMPANY WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string CompanyCRUD(Company cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(COMPANY_CODE) as cnt FROM COMPANY WHERE COMPANY_CODE = LTRIM(RTRIM('" + cy.Code + "')) and COMPANY_NAME = LTRIM(RTRIM('" + cy.CompanyName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Company Name Already Existed";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("CompanyProc", objConn);
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
                    objCmd.Parameters.Add("@companycode", SqlDbType.NVarChar).Value = cy.Code;
                    objCmd.Parameters.Add("@companyname", SqlDbType.NVarChar).Value = cy.CompanyName;
                    objCmd.Parameters.Add("@address1", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = cy.State;
                    objCmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = cy.Country;
                    objCmd.Parameters.Add("@mobile", SqlDbType.NVarChar).Value = cy.Mobile;
                    objCmd.Parameters.Add("@telephone", SqlDbType.NVarChar).Value = cy.Landline;
                    objCmd.Parameters.Add("@fax", SqlDbType.NVarChar).Value = cy.Email;
                    objCmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = cy.Fax;
                    objCmd.Parameters.Add("@Gst", SqlDbType.NVarChar).Value = cy.CST;
                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = "CreateBy";
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = "UpdateBy";
                        objCmd.Parameters.Add("@updatedon", SqlDbType.Date).Value = DateTime.Now;
                    }
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE COMPANY SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE COMPANY SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
