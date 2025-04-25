using RetailSales.Interface;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services
{
    public class CityServices : ICityService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CityServices(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }

        // used for country binding and retrieving from database

        public DataTable GetCountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRY.ID,COUNTRY.COUNTRY_NAME,COUNTRY.IS_ACTIVE from COUNTRY  WHERE COUNTRY.IS_ACTIVE='Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // used for state binding and retrieving from database

        public DataTable GetState(string stateid)
        {
            string SvSql = string.Empty;
            SvSql = "select STATE.ID,STATE.STATE_NAME,STATE.IS_ACTIVE from STATE  WHERE COUNTRY_ID = '" + stateid + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // retrieving from database

        public DataTable GetAllCityGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT CITY.ID,CITY.CITY_NAME,COUNTRY.COUNTRY_NAME,STATE.STATE_NAME,CITY.IS_ACTIVE FROM CITY LEFT OUTER JOIN COUNTRY ON COUNTRY.ID=CITY.COUNTRY_ID LEFT OUTER JOIN STATE ON STATE.ID=CITY.STATE_ID WHERE CITY.IS_ACTIVE = 'Y' ORDER BY CITY.ID DESC";
            }
            else
            {
                SvSql = "SELECT CITY.ID,CITY.CITY_NAME,COUNTRY.COUNTRY_NAME,STATE.STATE_NAME,CITY.IS_ACTIVE FROM CITY LEFT OUTER JOIN COUNTRY ON COUNTRY.ID=CITY.COUNTRY_ID LEFT OUTER JOIN STATE ON STATE.ID=CITY.STATE_ID WHERE CITY.IS_ACTIVE = 'N' ORDER BY CITY.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // edit and store to database

        public DataTable GetEditCityDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,CITY_NAME,STATE_ID,COUNTRY_ID FROM CITY WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        // data deletion. data moves from enabled to disabled page
        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE CITY SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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

        // deleted data inclusion. data moves from disabled to enabled page
        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE CITY SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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

        // getting i/p and storing to database

        public string CityCRUD(City ic)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                if (ic.ID == null)
                {

                    svSQL = "SELECT Count(CITY_NAME) as cnt FROM CITY WHERE CITY_NAME = LTRIM(RTRIM('" + ic.CityName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "City Name Already Exist";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("CityProc", objConn);
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ic.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = ic.ID;
                    }
                    objCmd.Parameters.Add("@cityname", SqlDbType.NVarChar).Value = ic.CityName;
                    objCmd.Parameters.Add("@stateid", SqlDbType.NVarChar).Value = ic.StateId;
                    objCmd.Parameters.Add("@countryid", SqlDbType.NVarChar).Value = ic.CountryId;
                    if (ic.ID == null)
                    {
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = userId;
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = userId;
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
    }
}
