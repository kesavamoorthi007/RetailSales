using RetailSales.Interface.Purchase;
using RetailSales.Models;
using System.Data;
using System.Data.SqlClient;

namespace RetailSales.Services.Purchase
{
    public class GRNService : IGRNService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public GRNService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllListGRN(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT GRN_BASIC_ID,GRN_NO,CONVERT(varchar, GRN_BASIC.GRN_DATE, 106) AS GRN_DATE,SUP_NAME,NET,GRN_BASIC.IS_ACTIVE,PAYMENT_TAG FROM GRN_BASIC   WHERE GRN_BASIC.IS_ACTIVE = 'Y' ORDER BY GRN_BASIC.GRN_BASIC_ID DESC";
            }
            else
            {
                SvSql = "SELECT GRN_BASIC_ID,GRN_NO,CONVERT(varchar, GRN_BASIC.GRN_DATE, 106) AS GRN_DATE,SUP_NAME,NET,GRN_BASIC.IS_ACTIVE,PAYMENT_TAG FROM GRN_BASIC  WHERE GRN_BASIC.IS_ACTIVE = 'N' ORDER BY GRN_BASIC.GRN_BASIC_ID DESC";

            }
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        
        public DataTable GetGRN(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "SELECT PONO,FORMAT(POBASIC.PODATE, 'MM/dd/yyyy') AS PODATE,SUPPLIER.SUPPLIER_NAME,POBASIC.ADDRESS,POBASIC.COUNTRY,POBASIC.STATE,POBASIC.CITY,POBASIC.GST_NO,REF_NO,FORMAT(POBASIC.REF_DATE, 'MM/dd/yyyy') AS REF_DATE,POBASIC.GST_NO,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,FORMAT(POBASIC.LR_DATE, 'MM/dd/yyyy') AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM POBASIC LEFT OUTER JOIN SUPPLIER ON SUPPLIER.ID=POBASIC.SUP_NAME WHERE POBASIC.POBASICID='" + id + "'";
            SvSql = "SELECT GRN_NO,FORMAT(GRN_BASIC.GRN_DATE, 'MM/dd/yyyy') AS GRN_DATE,SUP_NAME,ADDRESS,COUNTRY,STATE,CITY,REF_NO,FORMAT(GRN_BASIC.REF_DATE, 'MM/dd/yyyy') AS REF_DATE,AMTINWORDS,NARRATION,TRANS_SPORTER,LR_NO,FORMAT(GRN_BASIC.LR_DATE, 'MM/dd/yyyy') AS LR_DATE,PLACE_DIS,GROSS,NET,CGST,SGST,IGST,ROUNT_OFF,DISCOUNT,FRIGHTCHARGE FROM GRN_BASIC WHERE GRN_BASIC.GRN_BASIC_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGRNItem(string id)
        {
            string SvSql = string.Empty;
            //SvSql = "SELECT POBASICID,PODETAILID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,PODETAIL.HSN,PODETAIL.TARIFF,PODETAIL.UOM,DEST_UOM,CONVT_FACTOR,QTY,CF_QTY,PODETAIL.RATE,AMOUNT,FRIGHT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM PODETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PODETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.ID=PODETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=PODETAIL.VARIANT WHERE PODETAIL.POBASICID='" + id + "'";
            SvSql = "SELECT GRN_BASIC_ID,GRN_DETAIL_ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,HSN,TARIFF,GRN_DETAIL.UOM,QTY,RECIVED_QTY,ACCEPTED_QTY,REJECTED_QTY,EXCEED_QTY,SHORT_QTY,DEST_UOM,CF,CF_QTY,GRN_DETAIL.RATE,AMOUNT,DISC_PER,DIS_AMOUNT,CGSTP,SGSTP,IGSTP,CGST,SGST,IGST,TOTAL_AMOUNT FROM GRN_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=GRN_DETAIL.ITEM LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=GRN_DETAIL.PRODUCT LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=GRN_DETAIL.VARIANT WHERE GRN_DETAIL.GRN_BASIC_ID='" + id + "'";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetShopBin()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BINMASTER.ID,BINMASTER.BINID,BINMASTER.IS_ACTIVE FROM BINMASTER WHERE LOCID='1007' AND BINMASTER.IS_ACTIVE = 'Y' ";
            DataTable dtt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SvSql, _connectionString);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGodownBin()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BINMASTER.ID,BINMASTER.BINID,BINMASTER.IS_ACTIVE FROM BINMASTER WHERE LOCID='2006' AND BINMASTER.IS_ACTIVE = 'Y' ";
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
                    svSQL = "UPDATE GRN_BASIC SET IS_ACTIVE ='N' WHERE GRN_BASIC_ID='" + id + "'";
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

        public string GRNACCOUNT(GRN cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                string Location = "12418000000423";
                string parentdocid = "";
                DataTable dtt = new DataTable();
                dtt = datatrans.GetData("select GRN_NO,GRN_DATE ,SUP_ID PARTYID from GRN_BASIC where GRN_BASIC_ID='" + cy.GRNID + "'");
                using (SqlConnection objConn = new SqlConnection(_connectionString))

                {
                    objConn.Open();

                    using (SqlCommand command = objConn.CreateCommand())
                    {
                        SqlTransaction transaction = objConn.BeginTransaction();
                        command.Transaction = transaction;

                        try
                            {
                                ////////////////////transaction
                                int t2cunt = 0;
                                double grossamt = 0;
                                string Grossledger = "";
                                double totgst = 0;
                                double netamt = 0;
                                foreach (GRNAccount cp in cy.Acclst)
                                {
                                    t2cunt += 1;
                                    if (cp.TypeName == "GROSS")
                                    {
                                        grossamt += cp.DRAmount;
                                        Grossledger = cp.Ledgername;
                                    }
                                    if (cp.TypeName == "CGST")
                                    {
                                        totgst += cp.DRAmount;
                                    }
                                    if (cp.TypeName == "SGST")
                                    {
                                        totgst += cp.DRAmount;
                                    }
                                    if (cp.TypeName == "IGST")
                                    {
                                        totgst += cp.DRAmount;
                                    }
                                    if (cp.TypeName == "NET")
                                    {
                                        netamt = cp.CRAmount;
                                    }
                                }
                                DataTable dtv = datatrans.GetData("select PREFIX,LAST_NUMBER LASTNO from Sequence where TRANSECTION_TYPE='vchpr' AND IS_ACTIVE='Y'");
                                string vNo = dtv.Rows[0]["PREFIX"].ToString() + dtv.Rows[0]["LASTNO"].ToString();
                                long vnno = Convert.ToInt64(dtv.Rows[0]["LASTNO"].ToString());
                                string mno = DateTime.Now.ToString("yyyyMM");

                                command.CommandText = "Insert into TRANS1 (T1SOURCEID,T1TYPE,T1VCHNO,CURRENCYID,EXCHANGERATE,T1REFNO,T1REFDT,MONTHNO,T1HIDBMID,T1HIDBAMT,T1HICRMID,T1HICRAMT,T1PARTYID,T1PARTYAMT,T1VCHDT,T1NARR,T2COUNT,VTYPE,AMTWD,USERNAME,MODIFIEDON,TOTGST) VALUES " +
                                    "('" + cy.GRNID + "','pu','" + vNo + "','1','1','" + dtt.Rows[0]["GRN_NO"].ToString() + "','" + dtt.Rows[0]["GRN_DATE"].ToString() + "','" + mno + "','" + Grossledger + "','" + grossamt + "','" + cy.mid + "','" + netamt + "','" + dtt.Rows[0]["PARTYID"].ToString() + "','" + netamt + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cy.Vmemo + "','" + t2cunt + "','R','" + cy.Amtinwords + "','" + cy.createdby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + totgst + "') SELECT SCOPE_IDENTITY();";
                                Object TRANS1 = command.ExecuteScalar();

                            foreach (GRNAccount cp in cy.Acclst)
                                {
                                    if (cp.IsDisable == true)
                                    {
                                        cp.Ledgername = cp.mid;
                                        cp.CRDR = cp.crdrh;
                                    }
                                    string mledger = "";
                                    if (cp.TypeName == "NET")
                                    {
                                        mledger = Grossledger;

                                    }
                                    else
                                    {
                                        mledger = cy.mid;
                                    }

                                    command.CommandText = "Insert into TRANS2 (TRANS1ID,DBCR,MID,NDBAMOUNT,DBAMOUNT,NCRAMOUNT,CRAMOUNT,EXRATE,SDBAMOUNT,SCRAMOUNT,T2VCHDT,T2TYPE,T2VCHSTATUS) VALUES " +
                                    "('" + TRANS1 + "','" + cp.CRDR + "','" + cp.Ledgername + "','" + cp.DRAmount + "','" + cp.DRAmount + "','" + cp.CRAmount + "','" + cp.CRAmount + "','1','" + cp.DRAmount + "','" + cp.CRAmount + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','pu','N')";
                                    command.ExecuteNonQuery();
                                    
                                }
                            
                                command.CommandText = " UPDATE Sequence SET LAST_NUMBER ='" + (vnno + 1).ToString() + "' where TRANSECTION_TYPE='vchpr' AND IS_ACTIVE='Y' ";
                                command.ExecuteNonQuery();

                                command.CommandText = "UPDATE GRN_BASIC SET PAYMENT_TAG ='1' WHERE GRN_BASIC_ID='" + cy.GRNID + "'";
                                command.ExecuteNonQuery();

                                ///////////////////transaction
                                transaction.Commit();
                            }
                            catch (DataException e)
                            {
                                transaction.Rollback();
                                Console.WriteLine(e.ToString());
                                Console.WriteLine("Neither record was written to database.");
                            }
                        }
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
