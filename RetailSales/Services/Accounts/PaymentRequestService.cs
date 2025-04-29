using System.Data;
using System.Data.SqlClient;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Models.Master;

namespace RetailSales.Services.Accounts
{
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentRequestService(IConfiguration _configuratio, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            _httpContextAccessor = httpContextAccessor;
        }

        public DataTable GetPaymentRequestItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT GRN_BASIC.SUP_ID,GRN_BASIC.GRN_NO,GRN_BASIC.NET FROM GRN_BASIC WHERE GRN_BASIC.GRN_BASIC_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGRNDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUP_ID, GRN_NO, NET FROM GRN_BASIC WHERE GRN_BASIC.SUP_ID='" + ItemId + "' AND GRN_BASIC.IS_ACTIVE = 'Y'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string PaymentRequestCRUD(PaymentRequest cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                
                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("PayReqProc", objConn);
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
                    objCmd.Parameters.Add("@suppname", SqlDbType.NVarChar).Value = cy.SuppName;
                    objCmd.Parameters.Add("@reqby", SqlDbType.NVarChar).Value = cy.ReqBy;
                    objCmd.Parameters.Add("@totreqamt", SqlDbType.NVarChar).Value = cy.TotReqAmt;
                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("@createdby", SqlDbType.NVarChar).Value = cy.ReqBy;
                        objCmd.Parameters.Add("@createdon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("@updatedby", SqlDbType.NVarChar).Value = cy.ReqBy;
                        objCmd.Parameters.Add("@updatedon", SqlDbType.Date).Value = DateTime.Now;
                    }
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.GRNlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Models.Accounts.GRNItem cp in cy.GRNlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into PAYREQDETAIL (PAYREQBASICID,GRN_NO,GRN_AMT,REQ_AMT,PEND_AMT) VALUES ('" + Pid + "','" + cp.GRNNo + "','" + cp.GRNAmt + "','" + cp.ReqAmt + "','" + cp.PendAmt + "') SELECT SCOPE_IDENTITY()";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete PAYREQDETAIL WHERE PAYREQBASICID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();

                                foreach (Models.Accounts.GRNItem cp in cy.GRNlst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PAYREQDETAIL (PAYREQBASICID,GRN_NO,GRN_AMT,REQ_AMT,PEND_AMT) VALUES ('" + Pid + "','" + cp.GRNNo + "','" + cp.GRNAmt + "','" + cp.ReqAmt + "','" + cp.PendAmt + "') SELECT SCOPE_IDENTITY()";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }
                                }
                            }

                        }




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

        public DataTable GetAllListPurchaseorder(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT PAYREQBASICID,SUPPLIER.SUPPLIER_NAME,REQ_BY,TOT_REQ_AMT,PAYREQBASIC.IS_ACTIVE FROM PAYREQBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=PAYREQBASIC.SUP_ID WHERE PAYREQBASIC.IS_ACTIVE = 'Y' ORDER BY PAYREQBASIC.PAYREQBASICID DESC";
            }
            else
            {
                SvSql = "SELECT PAYREQBASICID,SUPPLIER.SUPPLIER_NAME,REQ_BY,TOT_REQ_AMT,PAYREQBASIC.IS_ACTIVE FROM PAYREQBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=PAYREQBASIC.SUP_ID WHERE PAYREQBASIC.IS_ACTIVE = 'N' ORDER BY PAYREQBASIC.PAYREQBASICID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPaymentRequest(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUPPLIER.SUPPLIER_NAME,REQ_BY,TOT_REQ_AMT FROM PAYREQBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=PAYREQBASIC.SUP_ID WHERE PAYREQBASIC.PAYREQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPaymentRequestTable(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PAYREQBASICID,GRN_NO,GRN_AMT,REQ_AMT,PEND_AMT FROM PAYREQDETAIL WHERE PAYREQDETAIL.PAYREQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditPaymentRequest(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SUP_ID,REQ_BY,TOT_REQ_AMT FROM PAYREQBASIC WHERE PAYREQBASIC.PAYREQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditPaymentRequestTable(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PAYREQBASICID,PAYREQDETAILID,GRN_NO,GRN_AMT,REQ_AMT,PEND_AMT FROM PAYREQDETAIL WHERE PAYREQDETAIL.PAYREQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string RemoveChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE PAYREQBASIC SET IS_ACTIVE ='Y' WHERE PAYREQBASICID='" + id + "'";
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

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (SqlConnection objConnT = new SqlConnection(_connectionString))
                {
                    svSQL = "UPDATE PAYREQBASIC SET IS_ACTIVE ='N' WHERE PAYREQBASICID='" + id + "'";
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
