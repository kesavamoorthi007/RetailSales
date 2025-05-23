﻿using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Master
{
    public class CompanyService : ICompanyService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CompanyService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }
        public DataTable GetAllCompanyGRID(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT COMPANY.ID,COMPANY_NAME,ADDRESS,CITY.CITY_NAME,STATE.STATE_NAME,COUNTRY.COUNTRY_NAME,COMPANY.IS_ACTIVE FROM COMPANY LEFT OUTER JOIN COUNTRY ON COUNTRY.ID = COMPANY.COUNTRY LEFT OUTER JOIN STATE ON STATE.ID=COMPANY.STATE LEFT OUTER JOIN CITY ON CITY.ID = COMPANY.CITY  WHERE COMPANY.IS_ACTIVE = 'Y' ORDER BY COMPANY.ID DESC";
            }
            else
            {
                SvSql = "SELECT COMPANY.ID,COMPANY_NAME,ADDRESS,CITY.CITY_NAME,STATE.STATE_NAME,COUNTRY.COUNTRY_NAME,COMPANY.IS_ACTIVE FROM COMPANY LEFT OUTER JOIN COUNTRY ON COUNTRY.ID = COMPANY.COUNTRY LEFT OUTER JOIN STATE ON STATE.ID=COMPANY.STATE LEFT OUTER JOIN CITY ON CITY.ID = COMPANY.CITY  WHERE COMPANY.IS_ACTIVE = 'N' ORDER BY COMPANY.ID DESC";

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
        public DataTable GetEditCompanyDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ID,COMPANY_NAME,ADDRESS,MOBILE_NO,COUNTRY,STATE,CITY,EMAIL_ID,LAND_LINE,WEBSITE FROM COMPANY WHERE ID = '" + id + "' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string CompanyCRUD(Company cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                var userId = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                if (cy.ID == null)
                {

                    svSQL = "SELECT Count(COMPANY_NAME) as cnt FROM COMPANY WHERE COMPANY_NAME = LTRIM(RTRIM('" + cy.CompanyName + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Company Name Already Exist";
                        return msg;
                    }
                }
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("CompanyProc", objConn);
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
                    objCmd.Parameters.Add("@companyname", SqlDbType.NVarChar).Value = cy.CompanyName;
                    objCmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = cy.Address;
                    objCmd.Parameters.Add("@mobile", SqlDbType.NVarChar).Value = cy.Mobile;
                    objCmd.Parameters.Add("@country", SqlDbType.NVarChar).Value = cy.Country;
                    objCmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = cy.State;
                    objCmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = cy.City;
                    objCmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = cy.Email;
                    objCmd.Parameters.Add("@landline", SqlDbType.NVarChar).Value = cy.Landline;
                    objCmd.Parameters.Add("@website", SqlDbType.NVarChar).Value = cy.Website;
                    if (cy.ID == null)
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
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE COMPANY SET IS_ACTIVE ='N' WHERE ID='" + id + "'";
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
                    svSQL = "UPDATE COMPANY SET IS_ACTIVE = 'Y' WHERE ID='" + id + "'";
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
