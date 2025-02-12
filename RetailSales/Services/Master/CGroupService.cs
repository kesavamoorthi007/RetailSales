using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class CGroupService : ICGroupService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllCGroupGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT CUSTOMER_GROUP_MASTER.ID,CUSTOMER_GROUP_MASTER.CUSTOMER_GROUP,CUSTOMER_GROUP_MASTER.DESCRIPTION,CUSTOMER_GROUP_MASTER.STATUS FROM CUSTOMER_GROUP_MASTER WHERE CUSTOMER_GROUP_MASTER.STATUS = 'Y' ORDER BY CUSTOMER_GROUP_MASTER.ID DESC";
            }
            else
            {
                SvSql = "SELECT CUSTOMER_GROUP_MASTER.ID,CUSTOMER_GROUP_MASTER.CUSTOMER_GROUP,CUSTOMER_GROUP_MASTER.DESCRIPTION,CUSTOMER_GROUP_MASTER.STATUS FROM CUSTOMER_GROUP_MASTER WHERE CUSTOMER_GROUP_MASTER.STATUS = 'N' ORDER BY CUSTOMER_GROUP_MASTER.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string CGroupCRUD(CGroup Ic)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (Ic.ID == null)
                {

                    svSQL = "SELECT Count(CUSTOMER_GROUP) as cnt FROM CUSTOMER_GROUP_MASTER WHERE CUSTOMER_GROUP = LTRIM(RTRIM('" + Ic.Customercategory + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Customer Group Name Already Existed";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("GroupProc", objConn);
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
                    objCmd.Parameters.Add("@customergroup", SqlDbType.NVarChar).Value = Ic.Customercategory;
                    objCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = Ic.Description;
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
        public DataTable GetEditCGroupDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,CUSTOMER_GROUP,DESCRIPTION FROM CUSTOMER_GROUP_MASTER WHERE ID = '" + id + "' ";
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
                    svSQL = "UPDATE CUSTOMER_GROUP_MASTER SET STATUS ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE CUSTOMER_GROUP_MASTER SET STATUS = 'Y' WHERE ID='" + id + "'";
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
