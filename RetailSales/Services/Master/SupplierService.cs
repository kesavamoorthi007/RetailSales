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

        public DataTable GetEditSupplier(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,SUPPLIER_NAME,SUPP_CAT,DEL_LEAD_TIME,CR_DAYS,MOBILE_NO,ADDRESS,CITY,EMAIL_ID,LANDLINE_NO FROM SUPPLIER WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllSupplierGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT SUPPLIER.ID,SUPPLIER_NAME,SUPP_CAT,DEL_LEAD_TIME,CR_DAYS,MOBILE_NO,ADDRESS,CITY,EMAIL_ID,LANDLINE_NO,SUPPLIER.IS_ACTIVE FROM SUPPLIER WHERE SUPPLIER.IS_ACTIVE = 'Y' ORDER BY SUPPLIER.ID DESC";
            }
            else
            {
                SvSql = "SELECT SUPPLIER.ID,SUPPLIER_NAME,SUPP_CAT,DEL_LEAD_TIME,CR_DAYS,MOBILE_NO,ADDRESS,CITY,EMAIL_ID,LANDLINE_NO,SUPPLIER.IS_ACTIVE FROM SUPPLIER WHERE SUPPLIER.IS_ACTIVE = 'N' ORDER BY SUPPLIER.ID DESC";

            }
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
                    objCmd.Parameters.Add("@deliveryleadtime", SqlDbType.NVarChar).Value = cy.Delivery;
                    objCmd.Parameters.Add("@mobilenumber", SqlDbType.NVarChar).Value = cy.Mobile;
                    objCmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
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

        //public string StatusDeleteMR(string tag, String id)
        //{

        //    try
        //    {
        //        string svSQL = string.Empty;
        //        using (SqlConnection objConnT = new SqlConnection(_connectionString))
        //        {
        //            svSQL = "UPDATE SUPPLIER SET deletenews ='N' WHERE I_Id='" + id + "'";
        //            SqlCommand objCmds = new SqlCommand(svSQL, objConnT);
        //            objConnT.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConnT.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return "";
        //}
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE Supplier SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
