using DocumentFormat.OpenXml.Spreadsheet;
using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RetailSales.Services.Master
{
    public class LocationService : ILocationService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public LocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllLocationGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "  SELECT ID,LOCATION_NAME,LOC_ADDRESS,BIN,LOCATION.IS_ACTIVE FROM LOCATION WHERE LOCATION.IS_ACTIVE = 'Y' ORDER BY LOCATION.ID DESC";
            }
            else
            {
                SvSql = "  SELECT ID,LOCATION_NAME,LOC_ADDRESS,BIN,LOCATION.IS_ACTIVE FROM LOCATION WHERE LOCATION.IS_ACTIVE = 'N' ORDER BY LOCATION.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditLocation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,LOCATION_NAME,LOC_ADDRESS,BIN FROM LOCATION WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string LocationCRUD(Models.Location cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

               
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("LocationProc", objConn);
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
                    objCmd.Parameters.Add("@locationname", SqlDbType.NVarChar).Value = cy.Locationname;
                    objCmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@bin", SqlDbType.NVarChar).Value = cy.Bin;
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
                    svSQL = "UPDATE LOCATION SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE LOCATION SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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


