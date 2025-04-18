using System.Data;
using System.Data.SqlClient;
using RetailSales.Interface.Master;
using RetailSales.Models;

namespace RetailSales.Services.Master
{
    public class SupplierService : ISupplierService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SupplierService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllSupplierGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SUPPLIER.ID,SUPPLIER_NAME,SUPPL_CATGRY.CATGRY_NAME,CR_DAYS,SUPPLIER.IS_ACTIVE FROM SUPPLIER LEFT OUTER JOIN SUPPL_CATGRY ON CAST(SUPPL_CATGRY.ID AS NVARCHAR) = SUPPLIER.SUPP_CAT WHERE SUPPLIER.IS_ACTIVE = 'Y' ORDER BY SUPPLIER.ID DESC";
            }
            else
            {
                SvSql = "SELECT SUPPLIER.ID,SUPPLIER_NAME,SUPPL_CATGRY.CATGRY_NAME,CR_DAYS,SUPPLIER.IS_ACTIVE FROM SUPPLIER LEFT OUTER JOIN SUPPL_CATGRY ON CAST(SUPPL_CATGRY.ID AS NVARCHAR) = SUPPLIER.SUPP_CAT WHERE SUPPLIER.IS_ACTIVE = 'N' ORDER BY SUPPLIER.ID DESC";

            }
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
        public DataTable GetCity(string cityid)
        {
            string SvSql = string.Empty;
            SvSql = "select CITY_NAME,ID from CITY WHERE STATE_ID = '" + cityid + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "select CATGRY_NAME,ID from SUPPL_CATGRY";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditSupplier(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,SUPPLIER_NAME,SUPP_CAT,CR_DAYS,MOBILE_NO,ADDRESS,CITY,STATE,GST_NO,EMAIL_ID,LANDLINE_NO FROM SUPPLIER WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPincodeDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select CITY_NAME,PINCODE,CITY.ID from CITY WHERE CITY.CITY_NAME='" + ItemId + "' AND CITY.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string SupplierCRUD(Supplier cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                //string statename = datatrans.GetDataString("SELECT STATE_NAME FROM STATE WHERE ID='"+cy.State+"'");
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("SupplierProc", objConn);
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
                    objCmd.Parameters.Add("@suppliername", SqlDbType.NVarChar).Value = cy.Supp;
                    objCmd.Parameters.Add("@suppliercategory", SqlDbType.NVarChar).Value = cy.Category;
                    //objCmd.Parameters.Add("@deliveryleadtime", SqlDbType.NVarChar).Value = cy.Delivery;
                    objCmd.Parameters.Add("@mobilenumber", SqlDbType.NVarChar).Value = cy.Mobile;
                    objCmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = cy.State;
                    //objCmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = cy.Country;
                    objCmd.Parameters.Add("@gst", SqlDbType.NVarChar).Value = cy.Gst;
                    objCmd.Parameters.Add("@emailid", SqlDbType.NVarChar).Value = cy.Email;
                    objCmd.Parameters.Add("@landlineno", SqlDbType.NVarChar).Value = cy.Landline;
                    objCmd.Parameters.Add("@creditdays", SqlDbType.NVarChar).Value = cy.Days;
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
                    svSQL = "UPDATE SUPPLIER SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE SUPPLIER SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
