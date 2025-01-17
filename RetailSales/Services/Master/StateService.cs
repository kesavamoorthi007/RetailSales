using System.Data;
using System.Data.SqlClient;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Services.Master
{
    public class StateService : IStateService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StateService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }



        public DataTable GetCountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRY.ID,COUNTRY.COUNTRY_NAME,COUNTRY.COUNTRY_CODE from COUNTRY where IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE.ID,STATE.STATE_NAME,STATE.STATE_CODE,STATE.COUNTRY_ID,STATE.CREATED_BY,STATE.CREATED_ON,STATE.UPDATED_BY,STATE.UPDATED_ON,STATE.IS_ACTIVE from STATE where IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllStateGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT STATE.IS_ACTIVE,STATE.ID,STATE.STATE_NAME,STATE.STATE_CODE,COUNTRY.COUNTRY_NAME FROM STATE  LEFT OUTER JOIN COUNTRY ON COUNTRY.ID = STATE.COUNTRY_ID WHERE STATE.IS_ACTIVE = 'Y' ORDER BY STATE.ID DESC";
            }
            else
            {
                SvSql = "SELECT STATE.IS_ACTIVE,STATE.ID,STATE.STATE_NAME,STATE.STATE_CODE,COUNTRY.COUNTRY_NAME FROM STATE  LEFT OUTER JOIN COUNTRY ON COUNTRY.ID = STATE.COUNTRY_ID WHERE STATE.IS_ACTIVE = 'N' ORDER BY STATE.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public string StateCRUD(Models.Master.State Ic)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (Ic.ID == null)
                {

                    svSQL = "SELECT Count(STATE_CODE) as cnt FROM STATE WHERE STATE_CODE = LTRIM(RTRIM('" + Ic.StatCode + "')) and STATE_NAME = LTRIM(RTRIM('" + Ic.StatName + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "State Name Already Existed";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("StateProc", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (Ic.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = Ic.ID;
                    }
                    objCmd.Parameters.Add("@statecode", SqlDbType.NVarChar).Value = Ic.StatCode;
                    objCmd.Parameters.Add("@statename", SqlDbType.NVarChar).Value = Ic.StatName;
                    objCmd.Parameters.Add("@countrycode", SqlDbType.NVarChar).Value = Ic.ConName;
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

        public DataTable GetEditStateDetail(string id)
        {

             
            string SvSql = string.Empty;
            SvSql = "SELECT ID,STATE_CODE,STATE_NAME,COUNTRY_ID FROM STATE WHERE ID = '" + id + "'";
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
                    svSQL = "UPDATE STATE SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE STATE SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
