using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class BankaccountsService : IBankaccountsService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BankaccountsService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllBankaccountsGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ID,ACC_NAME,ACC_NO,BANK_NAME,IS_ACTIVE FROM COMP_BANK_ACC WHERE COMP_BANK_ACC.IS_ACTIVE = 'Y' ORDER BY COMP_BANK_ACC.ID DESC";
            }
            else
            {
                SvSql = "SELECT ID,ACC_NAME,ACC_NO,BANK_NAME,IS_ACTIVE FROM COMP_BANK_ACC WHERE COMP_BANK_ACC.IS_ACTIVE = 'N' ORDER BY COMP_BANK_ACC.ID DESC";
            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditBankaccountsDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ACC_TYPE,ACC_NAME,ACC_NO,BANK_NAME,BRANCH_NAME,BRANCH_ADDR,BR_COUNTRY,BR_STATE,BR_CITY,BSR_CODE,IFSC_CODE FROM COMP_BANK_ACC WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAccounttype()
        {
            string SvSql = string.Empty;
            SvSql = "select ACC_TYPE_NAME,ID from ACC_TYPE";
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
        public string BankaccountsCRUD(Bankaccounts Cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    objConn.Open();
                    if (Cy.ID == null)
                    {
                        svSQL = "Insert into COMP_BANK_ACC (ACC_NAME,ACC_NO,BANK_NAME,ACC_TYPE,BRANCH_NAME,BRANCH_ADDR,BR_COUNTRY,BR_STATE,BR_CITY,BSR_CODE,IFSC_CODE) VALUES ('" + Cy.Accountname + "','" + Cy.Accountnumber + "','" + Cy.Bankname + "','" + Cy.Accounttype + "','" + Cy.Branchname + "','" + Cy.Branchaddress + "','" + Cy.Country + "','" + Cy.State + "','" + Cy.City + "','" + Cy.Bsrcode + "','" + Cy.Ifsccode + "')";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();

                    }
                    else
                    {
                        svSQL = "Update COMP_BANK_ACC set ACC_NAME = '" + Cy.Accountname + "',ACC_NO = '" + Cy.Accountnumber + "',BANK_NAME = '" + Cy.Bankname + "',ACC_TYPE = '" + Cy.Accounttype + "',BRANCH_NAME = '" + Cy.Branchname + "',BRANCH_ADDR = '" + Cy.Branchaddress + "',BR_COUNTRY = '" + Cy.Country + "',BR_STATE = '" + Cy.State + "',BR_CITY = '" + Cy.City + "',BSR_CODE = '" + Cy.Bsrcode + "',IFSC_CODE = '" + Cy.Ifsccode + "' WHERE COMP_BANK_ACC.A_Id ='" + Cy.ID + "'";
                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
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
                    svSQL = "UPDATE COMP_BANK_ACC SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE COMP_BANK_ACC SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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