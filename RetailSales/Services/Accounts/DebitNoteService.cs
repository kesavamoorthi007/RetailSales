using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Inventory;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Accounts
{
    public class DebitNoteService : IDebitNoteService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DebitNoteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public string DebitNoteCRUD(DebitNote cy)
        {
            string msg = "";
            try
            {
                int t2cunt = 0;
                double? grossamt = 0;
                string Depitledger = "";
                string Cretiledger = "";
                //double totgst = 0;
                double? netamt = 0;
                string ledger = string.Join("", cy.DebitNotelst.Where(x => x.DebitAmt == cy.DebitNotelst.Max(y => y.DebitAmt)).Select(x => x.AccName));
                Cretiledger = string.Join("", cy.DebitNotelst.Where(x => x.CreditAmt == cy.DebitNotelst.Max(y => y.CreditAmt)).Select(x => x.AccName));

                foreach (DebitNoteItem cp in cy.DebitNotelst)
                {
                    t2cunt += 1;
                    if (cp.DBCR == "Dr")
                    {
                        grossamt += cp.DebitAmt;
                    }

                    else
                    {
                        netamt += cp.CreditAmt;
                        //Cretiledger = cp.AccName;
                    }
                }
                string StatementType = string.Empty;
                string svSQL = "";

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    SqlCommand objCmd = new SqlCommand("AccVoucherProc", objConn);
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
                    objCmd.Parameters.Add("@vouchno", SqlDbType.NVarChar).Value = cy.VocNo;
                    objCmd.Parameters.AddWithValue("@vouchdate", cy.VocDate);
                    objCmd.Parameters.Add("@excrate", SqlDbType.NVarChar).Value = cy.ExcRate;
                    objCmd.Parameters.Add("@refno", SqlDbType.NVarChar).Value = cy.RefNo;
                    objCmd.Parameters.AddWithValue("@refdate", cy.RefDate);
                    objCmd.Parameters.Add("@currency", SqlDbType.NVarChar).Value = cy.Currency;
                    objCmd.Parameters.Add("@totdebamt", SqlDbType.NVarChar).Value = cy.Totdeb;
                    objCmd.Parameters.Add("@totcreamt", SqlDbType.NVarChar).Value = cy.Totcri;
                    objCmd.Parameters.Add("@amtinwords", SqlDbType.NVarChar).Value = cy.AmtWd;
                    objCmd.Parameters.Add("@narration", SqlDbType.NVarChar).Value = cy.Narr;
                    objCmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = StatementType;
                    try
                    {

                        objConn.Open();
                        Object Pid = objCmd.ExecuteScalar();
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }

                        if (cy.DebitNotelst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (DebitNoteItem cp in cy.DebitNotelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "INSERT INTO ACC_VOUCHER_DETAIL (ID,TRANS_TYPE,ACCOUNT_NAME,DEBIT_AMT,CREDIT_AMT,BALANCE) VALUES ('" + Pid + "','" + cp.DBCR + "','" + cp.AccName + "','" + cp.DebitAmt + "','" + cp.CreditAmt + "','" + cp.Balance + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "DELETE ACC_VOUCHER_DETAIL WHERE ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (DebitNoteItem cp in cy.DebitNotelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "INSERT INTO ACC_VOUCHER_DETAIL (ID,TRANS_TYPE,ACCOUNT_NAME,DEBIT_AMT,CREDIT_AMT,BALANCE) VALUES ('" + Pid + "','" + cp.DBCR + "','" + cp.AccName + "','" + cp.DebitAmt + "','" + cp.CreditAmt + "','" + cp.Balance + "')";
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

        public DataTable GetAcc()
        {
            string SvSql = string.Empty;
            SvSql = "Select LEDGER_NAME,ID FROM ACC_LEDGER  ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
