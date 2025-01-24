using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Accounts
{
    public class LedgersService : ILedgersServices
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public LedgersService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetLedgers()
        {
            string SvSql = string.Empty;
            SvSql = "select ACC_LEDGER.ID,ACC_LEDGER.ACC_GRP_CODE,ACC_LEDGER.LEDGER_NAME,ACC_LEDGER.ALLOW_ZERO_VAL,ACC_LEDGER.TOTAL_OPEN_BAL,ACC_LEDGER.LDGR_NOTES from ACC_LEDGER ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccountName()
        {
            string SvSql = string.Empty;
            SvSql = "select ACC_GROUP.ID,ACC_GROUP.ACC_GRP_NAME from ACC_GROUP";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditLedgersDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,ACC_GRP_CODE,LEDGER_NAME,ALLOW_ZERO_VAL,TOTAL_OPEN_BAL,LDGR_NOTES FROM ACC_LEDGER WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllLedgersGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ACC_LEDGER.ID,ACC_LEDGER.LEDGER_NAME,ACC_LEDGER.ALLOW_ZERO_VAL,ACC_LEDGER.TOTAL_OPEN_BAL,ACC_LEDGER.LDGR_NOTES,ACC_GROUP.ACC_GRP_NAME,ACC_LEDGER.IS_ACTIVE, FROM ACC_LEDGER LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_LEDGER.ACC_GRP_CODE LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_GROUP.ACC_GRP_NAME WHERE ACC_LEDGER.IS_ACTIVE = 'Y' ORDER BY ACC_LEDGER.ID DESC";
            }
            else
            {
                SvSql = "SELECT ACC_LEDGER.ID,ACC_LEDGER.LEDGER_NAME,ACC_LEDGER.ALLOW_ZERO_VAL,ACC_LEDGER.TOTAL_OPEN_BAL,ACC_LEDGER.LDGR_NOTES,ACC_GROUP.ACC_GRP_NAME,ACC_LEDGER.IS_ACTIVE, FROM ACC_LEDGER LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_LEDGER.ACC_GRP_CODE LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_GROUP.ACC_GRP_NAME WHERE ACC_LEDGER.IS_ACTIVE = 'Y' ORDER BY ACC_LEDGER.ID DESC";

            }

           // SvSql = "SELECT ACC_LEDGER.ID,ACC_LEDGER.LEDGER_NAME,ACC_LEDGER.ALLOW_ZERO_VAL,ACC_LEDGER.TOTAL_OPEN_BAL,ACC_LEDGER.LDGR_NOTES,ACC_GROUP.ACC_GRP_NAME,ACC_LEDGER.IS_ACTIVE, FROM ACC_LEDGER LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_LEDGER.ACC_GRP_CODE LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_GROUP.ACC_GRP_NAME WHERE ACC_LEDGER.IS_ACTIVE = 'Y' ORDER BY ACC_LEDGER.ID DESC";

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

        public string LedgersCRUD(Ledgers ic)
        {
            throw new NotImplementedException();
        }

        //public string LedgersCRUD(Ledgers ic)
        //{
        //    string msg = "";
        //    try
        //    {
        //        string StatementType = string.Empty;
        //        string svSQL = "";

        //        if (ic.ID == null)
        //        {

        //            svSQL = "SELECT Count(CITY_NAME) as cnt FROM CITY WHERE CITY_NAME = LTRIM(RTRIM('" + ic.CityName + "')) and STATE_ID = LTRIM(RTRIM('" + ic.StateId + "')) and COUNTRY_ID = LTRIM(RTRIM('" + ic.CountryId + "'))";
        //            if (datatrans.GetDataId(svSQL) > 0)
        //            {
        //                msg = "City Name Already Existed";
        //                return msg;
        //            }
        //        }
        //        using (SqlConnection objConn = new SqlConnection(_connectionString))
        //        {
        //            SqlCommand objCmd = new SqlCommand("CityProc", objConn);
        //            objCmd.CommandType = CommandType.StoredProcedure;
        //            if (ic.ID == null)
        //            {
        //                StatementType = "Insert";
        //                objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                StatementType = "Update";
        //                objCmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = ic.ID;
        //            }
        //            objCmd.Parameters.Add("@cityname", SqlDbType.NVarChar).Value = ic.CityName;
        //            objCmd.Parameters.Add("@stateid", SqlDbType.NVarChar).Value = ic.StateId;
        //            objCmd.Parameters.Add("@countryid", SqlDbType.NVarChar).Value = ic.CountryId;
        //            objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
        //            try
        //            {
        //                objConn.Open();
        //                objCmd.ExecuteNonQuery();

        //            }
        //            catch (Exception ex)
        //            {
        //                System.Console.WriteLine("Exception: {0}", ex.ToString());
        //            }
        //            objConn.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error Occurs, While inserting / updating Data";
        //        throw ex;
        //    }

        //    return msg;
        //}
    }

   
}
