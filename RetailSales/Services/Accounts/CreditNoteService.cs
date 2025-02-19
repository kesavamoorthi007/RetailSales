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

        public string CreditNoteCRUD(CreditNote cy)
        {
            string msg = "";
            try
            {
                //string StatementType = string.Empty; string svSQL = "";
                //datatrans = new DataTransactions(_connectionString);


                //int idc = datatrans.GetDataId("SELECT LAST_NUMBER FROM SEQUENCE WHERE PREFIX = 'GRN' AND IS_ACTIVE = 'Y'");
                //string docno = string.Format("{0}{1}{2}", "GRN/", "24-25/", (idc + 1).ToString());

                //string updateCMd = " UPDATE SEQUENCE SET LAST_NUMBER ='" + (idc + 1).ToString() + "' WHERE PREFIX ='GRN' AND IS_ACTIVE ='Y'";
                //try
                //{
                //    datatrans.UpdateStatus(updateCMd);
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}

                using (SqlConnection objConn = new SqlConnection(_connectionString))
                {
                    string SvSql1 = "Insert into ACC_VOUCHER (VOUCH_NO,VOUCH_DATE,EXCHANGE_RATE,REF_ID,REF_DATE,REF_NO,REF_DATE,CURRENCY,AMT,AMOUNT_IN_WORD,VOUCH_MEMO,IS_ACTIVE) VALUES ('" + cy.VocNo + "' ,'" + cy.VocDate + "','" + cy.exchangeRate + "','" + cy.RefNo + "','" + cy.RefDate + "','" + cy.Currency + "','" + cy.totamt + "','" + cy.AmtWd + "','" + cy.Narr + "','" + cy.ID + "','Y') RETURNING ID INTO :GRNDEID";
                    SqlCommand objCmdsss = new SqlCommand(SvSql1, objConn);
                    objCmdsss.Parameters.Add(new SqlParameter("GRNDEID", SqlDbType.Int));                   //'" + docno + "',
                    objCmdsss.Parameters["GRNDEID"].Direction = ParameterDirection.ReturnValue;
                    objConn.Open();
                    objCmdsss.ExecuteNonQuery();

                    string stkid = objCmdsss.Parameters["GRNDEID"].Value.ToString();
                    if (cy.ID != null)
                    {
                        stkid = cy.ID;
                    }
                    foreach (CreditNoteItem cp in cy.CreditNotelst)
                    {
                        string SvSql2 = "Insert into ACC_VOUCHER_DETAIL (VOUCH_BASIC_ID,TRANS_TYPE,ACCOUNT_NAME,DEBIT_AMT,CREDIT_AMT,BALANCE) VALUES ('" + stkid + "','" + cp.DBCR + "','" + cp.AccName + "','" + cp.DebitAmt + "','" + cp.CreditAmt + "','" + cp.Balance + "')";
                        SqlCommand objCmddts = new SqlCommand(SvSql2, objConn);
                        objConn.Open();
                        objCmddts.ExecuteNonQuery();

                    }

                    //svSQL = "Insert into GRN_BASIC (GRN_NO,GRN_DATE,SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,FRIGHTCHARGE,CGST,SGST,IGST,ROUNT_OFF,POBASIC_ID,IS_ACTIVE) (Select '" + docno + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "',SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,LR_DATE,PLACE_DIS,GROSS,NET,DISCOUNT,FRIGHTCHARGE,CGST,SGST,IGST,ROUNT_OFF,'" + cy.ID + "','Y' from POBASIC where POBASICID='" + cy.ID + "')";
                    //SqlCommand objCmd = new SqlCommand(svSQL, objConn);
                    //try
                    //{
                    //    objConn.Open();
                    //    objCmd.ExecuteNonQuery();
                    //}
                    //catch (Exception ex)
                    //{
                    //    //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    //}
                    //objConn.Close();
                }

                //string quotid = datatrans.GetDataString("Select GRN_BASIC_ID from GRN_BASIC Where POBASIC_ID=" + cy.ID + "");
                //using (SqlConnection objConnT = new SqlConnection(_connectionString))

                //{
                //    string Sql = "Insert into GRN_DETAIL (GRN_BASIC_ID,ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT) (Select '" + quotid + "',ITEM,VARIANT,HSN,TARIFF,UOM,QTY,RATE,AMOUNT,FRIGHT,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL WHERE POBASICID=" + cy.ID + ")";
                //    SqlCommand objCmds = new SqlCommand(Sql, objConnT);
                //    objConnT.Open();
                //    objCmds.ExecuteNonQuery();
                //    objConnT.Close();
                //}

                using (SqlConnection objConnE = new SqlConnection(_connectionString))
                {
                    string Sql = "UPDATE POBASIC SET STATUS='GRN Generated' where POBASICID='" + cy.ID + "'";
                    SqlCommand objCmds = new SqlCommand(Sql, objConnE);
                    objConnE.Open();
                    objCmds.ExecuteNonQuery();
                    objConnE.Close();
                }

            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }


    }

}
