using System.Data;
using System.Data.SqlClient;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Services.Master
{
    public class BINService : IBINService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BINService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public string BINCRUD(BIN cy)
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
                        svSQL = "Insert into BINMASTER (BINID,LOCID,BINDESC,CREATED_BY,CREATED_ON) VALUES ('" + cy.BINID + "','" + cy.Location + "','" + cy.Description + "','" + cy.Createdby + "','" + cy.Createdon + "')";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                        //StatementType = "Insert";
                        //objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        svSQL = "Update BINMASTER set BINID = '" + cy.BINID + "',BINDESC = '" + cy.Description + "',LOCID = '" + cy.Location + "',UPDATED_BY = '" + cy.Updatedby + "',UPDATED_ON = '" + cy.Updatedon + "' WHERE BINMASTER.ID ='" + cy.ID + "'";
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

        public DataTable GetAllBINGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT BINMASTER.ID,BINMASTER.BINID,BINMASTER.BINDESC,BINMASTER.LOCID,LOCATION.LOCATION_NAME,LOCATION.ID,BINMASTER.IS_ACTIVE,LOCATION.IS_ACTIVE FROM BINMASTER LEFT OUTER JOIN LOCATION ON BINMASTER.LOCID = LOCATION.ID WHERE BINMASTER.IS_ACTIVE = 'Y' ORDER BY BINMASTER.ID DESC";
            }// SvSql = "SELECT BINMASTER.ID,BINID,BINDESC,LOCID,BINMASTER.IS_ACTIVE FROM BINMASTER WHERE BINMASTER.IS_ACTIVE = 'Y' ORDER BY BINMASTER.ID DESC";

            else
            {
                SvSql = "SELECT BINMASTER.ID,BINMASTER.BINID,BINMASTER.BINDESC,BINMASTER.LOCID,LOCATION.LOCATION_NAME,LOCATION.ID,BINMASTER.IS_ACTIVE,LOCATION.IS_ACTIVE FROM BINMASTER LEFT OUTER JOIN LOCATION ON BINMASTER.LOCID = LOCATION.ID WHERE BINMASTER.IS_ACTIVE = 'N' ORDER BY BINMASTER.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditBIN(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,BINID,LOCID,BINDESC FROM BINMASTER WHERE ID = '" + id + "' ";
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

        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE BINMASTER SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE BINMASTER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
