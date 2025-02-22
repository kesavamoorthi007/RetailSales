using RetailSales.Interface.Accounts;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Accounts
{
    public class CreditNoteService : ICreditNoteService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CreditNoteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAcc(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select LEDGER_NAME,ID FROM ACC_LEDGER WHERE ACC_GRP_CODE ='026' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetLedgerDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT CLOSE_BAL FROM ACC_LEDGER  WHERE ID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string CreditNoteCRUD(CreditNote cy)
        {
            string msg = "";
            try
            {
                int t2cunt = 0;
                double? grossamt = 0;
                string Depitledger = "";
                string Cretiledger = "";
                double totgst = 0;
                double? netamt = 0;
                string ledger = string.Join("", cy.CreditNotelst.Where(x => x.DebitAmt == cy.CreditNotelst.Max(y => y.DebitAmt)).Select(x => x.AccName));
                Cretiledger = string.Join("", cy.CreditNotelst.Where(x => x.CreditAmt == cy.CreditNotelst.Max(y => y.CreditAmt)).Select(x => x.AccName));

                foreach (CreditNoteItem cp in cy.CreditNotelst)
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

                if (cy.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'PO' AND IS_ACTIVE = 'Y'");
                    string VocNo = string.Format("{0}{1}{2}", "PO/", "24-25/", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PO' AND IS_ACTIVE ='Y'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cy.VocNo = VocNo;
                }

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
                    //objCmd.Parameters.Add("@excrate", SqlDbType.NVarChar).Value = cy.ExcRate;
                    //objCmd.Parameters.Add("@refno", SqlDbType.NVarChar).Value = cy.RefNo;
                    //objCmd.Parameters.AddWithValue("@refdate", cy.RefDate);
                    //objCmd.Parameters.Add("@currency", SqlDbType.NVarChar).Value = cy.Currency;
                    objCmd.Parameters.Add("@totdebamt", SqlDbType.NVarChar).Value = cy.totdeb;
                    //objCmd.Parameters.Add("@totcreamt", SqlDbType.NVarChar).Value = cy.totcri;
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


                        if (cy.CreditNotelst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (CreditNoteItem cp in cy.CreditNotelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into ACC_VOUCHER_DETAIL (VOUCH_BASIC_ID,TRANS_TYPE,ACCOUNT_NAME,AMT,BALANCE) VALUES ('" + Pid + "','" + cp.DBCR + "','" + cp.AccName + "','" + cp.CreditAmt + "','" + cp.Balance + "')";
                                        SqlCommand objCmds = new SqlCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                svSQL = "Delete ACC_VOUCHER_DETAIL WHERE VOUCH_BASIC_ID='" + cy.ID + "'";
                                SqlCommand objCmdd = new SqlCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (CreditNoteItem cp in cy.CreditNotelst)
                                {

                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into ACC_VOUCHER_DETAIL (VOUCH_BASIC_ID,TRANS_TYPE,ACCOUNT_NAME,AMT,BALANCE) VALUES ('" + Pid + "','" + cp.DBCR + "','" + cp.AccName + "','" + cp.CreditAmt + "','" + cp.Balance + "')";
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
        public DataTable GetEditCreditNote(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PONO,CONVERT(varchar, POBASIC.PODATE, 106) AS PODATE,SUP_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,REF_NO,CONVERT(varchar, POBASIC.REF_DATE, 106) AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,CONVERT(varchar, POBASIC.LR_DATE, 106) AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC WHERE POBASIC.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        
        public DataTable GetCreditNoteDetailes(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT POBASICID,PODETAILID,PRODUCT.PRODUCT_NAME,PRO_DETAIL.PRODUCT_VARIANT,HSN,PODETAIL.TARIFF,PODETAIL.UOM,QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT WHERE PODETAIL.POBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }

}
