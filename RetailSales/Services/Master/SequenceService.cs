using DocumentFormat.OpenXml.Spreadsheet;
using Nest;
using RetailSales.Interface;
using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RetailSales.Services
{
    public class SequenceService : ISequenceService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SequenceService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllSequenceGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "  SELECT ID,TRANSECTION_TYPE,PREFIX,SUFFIX,LAST_NUMBER,NUMBER_LENGTH,SEQUENCE.IS_ACTIVE FROM SEQUENCE WHERE SEQUENCE.IS_ACTIVE = 'Y' ORDER BY SEQUENCE.ID DESC";
            }
            else
            {
                SvSql = "  SELECT ID,TRANSECTION_TYPE,PREFIX,SUFFIX,LAST_NUMBER,NUMBER_LENGTH,SEQUENCE.IS_ACTIVE FROM SEQUENCE WHERE SEQUENCE.IS_ACTIVE = 'N' ORDER BY SEQUENCE.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditSequence(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,TRANSECTION_TYPE,PREFIX,SUFFIX,LAST_NUMBER,NUMBER_LENGTH FROM SEQUENCE WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SequenceCRUD(Sequence cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";


                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("sequecneProc", objConn);
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
                    objCmd.Parameters.Add("@transectiontype", SqlDbType.NVarChar).Value = cy.Transection;
                    objCmd.Parameters.Add("@prefix", SqlDbType.NVarChar).Value = cy.Prefix;
                    objCmd.Parameters.Add("@suffix", SqlDbType.NVarChar).Value = cy.Suffix;
                    objCmd.Parameters.Add("@lastnumber", SqlDbType.NVarChar).Value = cy.Lnumber;
                    objCmd.Parameters.Add("@numberlength", SqlDbType.NVarChar).Value = cy.Number;
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
                    svSQL = "UPDATE SEQUENCE SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE SEQUENCE SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
