using Irony.Parsing.Construction;
using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;


namespace RetailSales.Services.Master
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public EmployeeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllEmployeeGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT USER_REGIST.ID,USER_REGIST.EMPLOYEE_ID,USER_REGIST.FNAME,USER_REGIST.GENDER,USER_REGIST.MOBILE,USER_REGIST.EMAIL,USER_REGIST.IS_ACTIVE FROM USER_REGIST WHERE USER_REGIST.IS_ACTIVE = 'Y' ORDER BY USER_REGIST.ID DESC";
            }
            else
            {
                SvSql = "SELECT USER_REGIST.ID,USER_REGIST.EMPLOYEE_ID,USER_REGIST.FNAME,USER_REGIST.GENDER,USER_REGIST.MOBILE,USER_REGIST.EMAIL,USER_REGIST.IS_ACTIVE FROM USER_REGIST WHERE USER_REGIST.IS_ACTIVE = 'N' ORDER BY USER_REGIST.ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusDeleteMR(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE USER_REGIST SET deletenews ='N' WHERE I_Id='" + id + "'";
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
        public string EmployeeCRUD(Employee cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                if (cy.ID == null)
                {                            

                   
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("EmployeeProc", objConn);
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
                 
                    objCmd.Parameters.Add("@EmpId", SqlDbType.NVarChar).Value = cy.EmpId;
                    objCmd.Parameters.Add("@fname", SqlDbType.NVarChar).Value = cy.Ename;
                    objCmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = cy.Address;
                    //objCmd.Parameters.Add("@branchid", SqlDbType.NVarChar).Value = cy.Branch;
                    //objCmd.Parameters.Add("@Departmentid", SqlDbType.NVarChar).Value = cy.Department;
                    //objCmd.Parameters.Add("@Desig", SqlDbType.NVarChar).Value = cy.Designation;
                    objCmd.Parameters.Add("@gender", SqlDbType.NVarChar).Value = cy.Gender;
                    objCmd.Parameters.Add("@MaritalStatus", SqlDbType.NVarChar).Value = cy.Maritalstatus;
                    objCmd.Parameters.Add("@cityname", SqlDbType.NVarChar).Value = cy.Cityid;
                     objCmd.Parameters.Add("@statename", SqlDbType.NVarChar).Value = cy.Stateid;
                    //objCmd.Parameters.Add("@Countryid", SqlDbType.NVarChar).Value = cy.Stateid;
                     objCmd.Parameters.Add("@mobile", SqlDbType.NVarChar).Value = cy.Mobile;
                     objCmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = cy.Email;
                    //objCmd.Parameters.Add("@emailpersonal", SqlDbType.NVarChar).Value = cy.Emailpersonal;
                    objCmd.Parameters.Add("@Degree", SqlDbType.NVarChar).Value = cy.Degree;
                      objCmd.Parameters.Add("@DateOfJoin", SqlDbType.NVarChar).Value = cy.Djoining;
                        objCmd.Parameters.Add("@DateOfLeave", SqlDbType.NVarChar).Value = cy.Dleaving;
                    objCmd.Parameters.Add("@DateOfBirth", SqlDbType.NVarChar).Value = cy.Dbirth;
                   // objCmd.Parameters.Add("@EmployeeStatus", SqlDbType.NVarChar).Value = cy.EmployeeStatus;
                    objCmd.Parameters.Add("@aadharnumber", SqlDbType.NVarChar).Value = cy.AadharNumber;
                    objCmd.Parameters.Add("@bank", SqlDbType.NVarChar).Value = cy.Bank;
                    objCmd.Parameters.Add("@accnumber", SqlDbType.NVarChar).Value = cy.AccNumber;

                    //objCmd.Parameters.Add("@ReportTo", SqlDbType.NVarChar).Value = cy.Report;

                    //objCmd.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = cy.Remark;
                    //objCmd.Parameters.Add("@Approved", SqlDbType.NVarChar).Value = cy.Approved;
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
        public DataTable GetEditEmployeeDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,EMPLOYEE_ID,FNAME,GENDER,ADDRESS,CITY_ID,STATE_ID,COUNTRY_ID,MOBILE,EMAIL,REMARKS,APPROVED_BY,BRANCH,DEPARTMENT,MARITALSTATUS,EMAILPERSONAL,DATEOFJOINING,DATEOFBIRTH,DATEOFLEAVING,DEGREE,EMPLOYEE_STATUS,REPORT_TO,AANUMBER,ACCNUMBER,BANK FROM USER_REGIST WHERE ID = '" + id + "' ";
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE USER_REGIST SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE USER_REGIST SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
