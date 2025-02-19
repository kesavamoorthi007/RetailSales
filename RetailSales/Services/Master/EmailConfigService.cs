using System.Data;
using System.Data.SqlClient;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Services.Master
{
    public class EmailConfigService : IEmailConfigService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public EmailConfigService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public string EmailConfigCRUD(EmailConfig cy)
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
                        svSQL = "INSERT INTO EMAIL_CONFIG (SMTP_HOST,PORT_NO,EMAIL_ID,PASSWORD,SSL,SIGNATURE,CREATED_BY,CREATED_ON) VALUES ('" + cy.Smtphost + "','" + cy.Portno + "','" + cy.Emailid + "','" + cy.Password + "','" + cy.SSL + "','" + cy.Signature + "','" + cy.CreatedBy + "','" + cy.CreatedOn + "')";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                    }
                    else
                    {
                        svSQL = "UPDATE EMAIL_CONFIG SET SMTP_HOST = '" + cy.Smtphost + "',PORT_NO = '" + cy.Portno + "',EMAIL_ID = '" + cy.Emailid + "',PASSWORD = '" + cy.Password + "',SSL = '" + cy.SSL + "',SIGNATURE = '" + cy.Signature + "',UPDATED_BY = '" + cy.UpdatedBy + "',UPDATED_ON = '" + cy.UpdatedOn + "' WHERE EMAIL_CONFIG.ID ='" + cy.ID + "'";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

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

        public DataTable GetAllEmailConfigGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT EMAIL_CONFIG.ID,SMTP_HOST,PORT_NO,EMAIL_ID,EMAIL_CONFIG.IS_ACTIVE FROM EMAIL_CONFIG WHERE EMAIL_CONFIG.IS_ACTIVE = 'Y' ORDER BY EMAIL_CONFIG.ID DESC";
            }

            else
            {
                SvSql = "SELECT EMAIL_CONFIG.ID,SMTP_HOST,PORT_NO,EMAIL_ID,EMAIL_CONFIG.IS_ACTIVE FROM EMAIL_CONFIG WHERE EMAIL_CONFIG.IS_ACTIVE = 'N' ORDER BY EMAIL_CONFIG.ID DESC";
            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditEmailConfig(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,SMTP_HOST,PORT_NO,EMAIL_ID,PASSWORD,SSL,SIGNATURE FROM EMAIL_CONFIG WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE EMAIL_CONFIG SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE EMAIL_CONFIG SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
