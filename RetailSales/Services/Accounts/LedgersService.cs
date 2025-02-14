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

        //public DataTable GetLedgers()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select ACC_LEDGER.ID,ACC_LEDGER.ACC_GRP_CODE,ACC_LEDGER.LEDGER_NAME,ACC_LEDGER.ALLOW_ZERO_VAL,ACC_LEDGER.TOTAL_OPEN_BAL,ACC_LEDGER.LDGR_NOTES from ACC_LEDGER ";
        //    DataTable dtt = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

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
                //SvSql = "SELECT SUPPLIER.ID,SUPPLIER_NAME,SUPPL_CATGRY.CATGRY_NAME,CR_DAYS,SUPPLIER.IS_ACTIVE FROM SUPPLIER LEFT OUTER JOIN SUPPL_CATGRY ON CAST(SUPPL_CATGRY.ID AS NVARCHAR) = SUPPLIER.SUPP_CAT WHERE SUPPLIER.IS_ACTIVE = 'Y' ORDER BY SUPPLIER.ID DESC";
                SvSql = "SELECT ACC_LEDGER.ID, ACC_GROUP.ACC_GRP_NAME,LEDGER_NAME,ACC_LEDGER.IS_ACTIVE FROM ACC_LEDGER LEFT OUTER JOIN ACC_GROUP ON CAST(ACC_GROUP.ACC_GRP_CODE AS NVARCHAR) = ACC_LEDGER.ACC_GRP_CODE WHERE ACC_LEDGER.IS_ACTIVE = 'Y' ORDER BY ACC_LEDGER.ID DESC";
                //SvSql = "select L.ID,L.IS_ACTIVE,LEDGER_NAME,ACC_GRP_NAME,G.ACC_CLASS,T.ACC_TYPE_NAME from ACC_LEDGER L LEFT OUTER JOIN ACC_GROUP G ON G.ACC_GRP_CODE=L.ACC_GRP_CODE LEFT OUTER JOIN ACC_TYPE T on G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE L.IS_ACTIVE='Y' ORDER BY L.ID DESC";
            }
            else
            {
                SvSql = "select L.ID,L.IS_ACTIVE,LEDGER_NAME,ACC_GRP_NAME,G.ACC_CLASS,T.ACC_TYPE_NAME from ACC_LEDGER L LEFT OUTER JOIN ACC_GROUP G ON G.ACC_GRP_CODE=L.ACC_GRP_CODE LEFT OUTER JOIN ACC_TYPE T on G.ACC_TYPE_CODE=T.ACC_TYPE_CODE WHERE L.IS_ACTIVE='N' ORDER BY L.ID DESC";

            }

           // SvSql = "SELECT ACC_LEDGER.ID,ACC_LEDGER.LEDGER_NAME,ACC_LEDGER.ALLOW_ZERO_VAL,ACC_LEDGER.TOTAL_OPEN_BAL,ACC_LEDGER.LDGR_NOTES,ACC_GROUP.ACC_GRP_NAME,ACC_LEDGER.IS_ACTIVE, FROM ACC_LEDGER LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_LEDGER.ACC_GRP_CODE LEFT OUTER JOIN ACC_GROUP ON ACC_GROUP.ID=ACC_GROUP.ACC_GRP_NAME WHERE ACC_LEDGER.IS_ACTIVE = 'Y' ORDER BY ACC_LEDGER.ID DESC";

            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string LedgersCRUD(Ledgers cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("LedgersProc", objConn);
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


                    objCmd.Parameters.Add("@accgroup", SqlDbType.NVarChar).Value = cy.AccountGroup;
                    objCmd.Parameters.Add("@ledgername", SqlDbType.NVarChar).Value = cy.LedgerName;
                    objCmd.Parameters.Add("@allowzerovalue", SqlDbType.NVarChar).Value = cy.AllowZeroValue;
                    objCmd.Parameters.Add("@totopenbal", SqlDbType.NVarChar).Value = cy.TotalOpeningBalance;
                    objCmd.Parameters.Add("@ledgernotes", SqlDbType.NVarChar).Value = cy.LedgerNotes;


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
                    svSQL = "UPDATE ACC_LEDGER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE ACC_LEDGER SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
