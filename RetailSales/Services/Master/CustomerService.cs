using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class CustomerService : ICustomerService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CustomerService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllCustomerGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "  SELECT Customer.ID,CUSTOMER_NAME,CUSTOMER_CODE,CUSTOMER_CATEGORY_MASTER.CUSTOMER_CATEGORY,CUSTOMER.DESCRIPTION,ADDRESS,COUNTRY,STATE,CITY,PHONE_NO,E_MAIL,GST_NO,CUSTOMER.IS_ACTIVE  FROM CUSTOMER LEFT OUTER JOIN CUSTOMER_CATEGORY_MASTER ON CUSTOMER_CATEGORY_MASTER.ID = CUSTOMER.CUSTOMER_CATEGORY   WHERE CUSTOMER.IS_ACTIVE = 'Y' ORDER BY CUSTOMER.ID DESC";
            }
            else
            {
                SvSql = "   SELECT Customer.ID,CUSTOMER_NAME,CUSTOMER_CODE,CUSTOMER_CATEGORY_MASTER.CUSTOMER_CATEGORY,CUSTOMER.DESCRIPTION,ADDRESS,COUNTRY,STATE,CITY,PHONE_NO,E_MAIL,GST_NO,CUSTOMER.IS_ACTIVE  FROM CUSTOMER LEFT OUTER JOIN CUSTOMER_CATEGORY_MASTER ON CUSTOMER_CATEGORY_MASTER.ID = CUSTOMER.CUSTOMER_CATEGORY   WHERE CUSTOMER.IS_ACTIVE = 'N' ORDER BY CUSTOMER.ID DESC";
            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRY_NAME,ID from COUNTRY";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE_NAME,ID from STATE";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCustomercategory()
        {
            string SvSql = string.Empty;
            SvSql = "select CUSTOMER_CATEGORY,ID from CUSTOMER_CATEGORY_MASTER";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCity()
        {
            string SvSql = string.Empty;
            SvSql = "select CITY_NAME,ID from CITY";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditCustomerDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,CUSTOMER_NAME,CUSTOMER_CODE,CUSTOMER_CATEGORY,DESCRIPTION,ADDRESS,COUNTRY,STATE,CITY,PHONE_NO,E_MAIL,GST_NO FROM CUSTOMER WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string CustomerCRUD(Customer cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(CUSTOMER_NAME) as cnt FROM CUSTOMER WHERE CUSTOMER_NAME = LTRIM(RTRIM('" + cy.CustomerName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Customer Name Already Existed";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("CustomerProc", objConn);
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
                    objCmd.Parameters.Add("@customername", SqlDbType.NVarChar).Value = cy.Customername;
                    objCmd.Parameters.Add("@customercode", SqlDbType.NVarChar).Value = cy.CustomerCode;
                    objCmd.Parameters.Add("@customercategory", SqlDbType.NVarChar).Value = cy.Customercategory;
                    objCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = cy.Description;
                    objCmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = cy.Country;
                    objCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = cy.State;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@Phoneno", SqlDbType.NVarChar).Value = cy.PhoneNo;
                    objCmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = cy.Email;
                    objCmd.Parameters.Add("@gst", SqlDbType.NVarChar).Value = cy.Gst;
                    
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
                    svSQL = "UPDATE CUSTOMER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE CUSTOMER SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
      

        
  

