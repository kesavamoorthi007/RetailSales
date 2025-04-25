using System.Data;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class UOMService : IUOMService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public UOMService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllUOMGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT UOM.ID,UOM_CODE,UOM_DESCRIPTION,CONVERSION_FACTOR,UOM.IS_ACTIVE FROM UOM WHERE UOM.IS_ACTIVE = 'Y' ORDER BY UOM.ID DESC";
            }
            else
            {
                SvSql = "SELECT UOM.ID,UOM_CODE,UOM_DESCRIPTION,CONVERSION_FACTOR,UOM.IS_ACTIVE FROM UOM WHERE UOM.IS_ACTIVE = 'N' ORDER BY UOM.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string UOMCRUD(UOM cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(UOM_CODE) as cnt FROM UOM WHERE UOM_CODE = LTRIM(RTRIM('" + cy.UOMCODE + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "UOM Code Already Exist";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    objConn.Open();
                    if (cy.ID == null)
                    {
                        svSQL = "Insert into UOM (UOM_CODE,UOM_DESCRIPTION,CONVERSION_FACTOR,CREATED_BY,CREATED_ON) VALUES ('" + cy.UOMCODE + "',N'" + cy.Description + "',N'" + cy.Factor + "','" + cy.Createdby + "','" + cy.Createdon + "')";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                        //StatementType = "Insert";
                        //objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        svSQL = "Update UOM set UOM_CODE = '" + cy.UOMCODE + "',UOM_DESCRIPTION = N'" + cy.Description + "',CONVERSION_FACTOR = N'" + cy.Factor + "',UPDATED_BY = '" + cy.Updatedby + "',UPDATED_ON = '" + cy.Updatedon + "' WHERE UOM.ID ='" + cy.ID + "'";
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

        public DataTable GetEditUOM(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,UOM_CODE,UOM_DESCRIPTION,CONVERSION_FACTOR FROM UOM WHERE ID = '" + id + "' ";
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
                    svSQL = "UPDATE UOM SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE UOM SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
