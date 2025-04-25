using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Models;
using RetailSales.Services.Purchase;
using System.Data;

namespace RetailSales.Controllers.Purchase
{
    public class GRNController : Controller
    {
        IGRNService GRNService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public GRNController(IGRNService _GRNService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            GRNService = _GRNService;
        }
        public IActionResult GRN()
        {
            Purchaseorder ic = new Purchaseorder();

            return View(ic);
        }
        public IActionResult ListGRN()
        {
            return View();
        }

        public IActionResult ViewGRN(string id)
        {
            GRN ic = new GRN();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = GRNService.GetGRN(id);
            if (dt.Rows.Count > 0)
            {
                ic.GRNNo = dt.Rows[0]["GRN_NO"].ToString();
                ic.GRNDate = dt.Rows[0]["GRN_DATE"].ToString();
                ic.SuppName = dt.Rows[0]["SUP_NAME"].ToString();
                ic.SuppAdd = dt.Rows[0]["ADDRESS"].ToString();
                ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                ic.State = dt.Rows[0]["STATE"].ToString();
                ic.City = dt.Rows[0]["CITY"].ToString();
                ic.RefNo = dt.Rows[0]["REF_NO"].ToString();
                ic.RefDate = dt.Rows[0]["REF_DATE"].ToString();
                ic.AmtInWords = dt.Rows[0]["AMTINWORDS"].ToString();
                ic.Narration = dt.Rows[0]["NARRATION"].ToString();
                ic.TransName = dt.Rows[0]["TRANS_SPORTER"].ToString();
                ic.LRNO = dt.Rows[0]["LR_NO"].ToString();
                ic.LRDate = dt.Rows[0]["LR_DATE"].ToString();
                ic.PlaceOfDis = dt.Rows[0]["PLACE_DIS"].ToString();
                ic.GrossAmt = dt.Rows[0]["GROSS"].ToString();
                ic.NetAmt = dt.Rows[0]["NET"].ToString();
                ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                ic.Fright = dt.Rows[0]["FRIGHTCHARGE"].ToString();
                ic.Cgst = dt.Rows[0]["CGST"].ToString();
                ic.Sgst = dt.Rows[0]["SGST"].ToString();
                ic.Igst = dt.Rows[0]["IGST"].ToString();
                ic.RoundAmt = dt.Rows[0]["ROUNT_OFF"].ToString();
                ic.ID = id;


            }

            List<GRNItem> TData = new List<GRNItem>();
            GRNItem tda = new GRNItem();



            dtt = GRNService.GetGRNItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new GRNItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Ordered = dtt.Rows[i]["QTY"].ToString();
                    tda.Recived = dtt.Rows[i]["RECIVED_QTY"].ToString();
                    tda.Accepted = dtt.Rows[i]["ACCEPTED_QTY"].ToString();
                    tda.Rejected = dtt.Rows[i]["REJECTED_QTY"].ToString();
                    tda.exqty = dtt.Rows[i]["EXCEED_QTY"].ToString();
                    tda.shortqty = dtt.Rows[i]["SHORT_QTY"].ToString();
                    tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CF"].ToString();
                    tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                    tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                    tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                    tda.CGST = dtt.Rows[i]["CGST"].ToString();
                    tda.SGST = dtt.Rows[i]["SGST"].ToString();
                    tda.IGST = dtt.Rows[i]["IGST"].ToString();
                    tda.DiscPer = dtt.Rows[i]["DISC_PER"].ToString();
                    tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
                    tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.GRNLst = TData;
            return View(ic);
        }

        public IActionResult MoveStock(string id)
        {
            GRN ic = new GRN();
            List<GRNItem> TData = new List<GRNItem>();
            GRNItem tda = new GRNItem();

            
            DataTable dt = new DataTable();
            dt = GRNService.GetGRN(id);
            if (dt.Rows.Count > 0)
            {
                ic.GRNNo = dt.Rows[0]["GRN_NO"].ToString();
                ic.GRNDate = dt.Rows[0]["GRN_DATE"].ToString();
                ic.SuppName = dt.Rows[0]["SUP_NAME"].ToString();
                ic.SuppAdd = dt.Rows[0]["ADDRESS"].ToString();
                ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                ic.State = dt.Rows[0]["STATE"].ToString();
                ic.City = dt.Rows[0]["CITY"].ToString();
                ic.RefNo = dt.Rows[0]["REF_NO"].ToString();
                ic.RefDate = dt.Rows[0]["REF_DATE"].ToString();
                //ic.AmtInWords = dt.Rows[0]["AMTINWORDS"].ToString();
                //ic.Narration = dt.Rows[0]["NARRATION"].ToString();
                //ic.TransName = dt.Rows[0]["TRANS_SPORTER"].ToString();
                //ic.LRNO = dt.Rows[0]["LR_NO"].ToString();
                //ic.LRDate = dt.Rows[0]["LR_DATE"].ToString();
                //ic.PlaceOfDis = dt.Rows[0]["PLACE_DIS"].ToString();
                //ic.GrossAmt = dt.Rows[0]["GROSS"].ToString();
                //ic.NetAmt = dt.Rows[0]["NET"].ToString();
                //ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                //ic.Fright = dt.Rows[0]["FRIGHTCHARGE"].ToString();
                //ic.Cgst = dt.Rows[0]["CGST"].ToString();
                //ic.Sgst = dt.Rows[0]["SGST"].ToString();
                //ic.Igst = dt.Rows[0]["IGST"].ToString();
                //ic.RoundAmt = dt.Rows[0]["ROUNT_OFF"].ToString();
                ic.ID = id;
            }
            DataTable dtt = new DataTable();
            dtt = GRNService.GetGRNItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new GRNItem();
                    tda.ShopBinlist = BindShopBin();
                    tda.GodownBinlist = BindGodownBin();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Tariff = dtt.Rows[i]["TARIFF"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Ordered = dtt.Rows[i]["QTY"].ToString();
                    tda.Recived = dtt.Rows[i]["RECIVED_QTY"].ToString();
                    //tda.Accepted = dtt.Rows[i]["ACCEPTED_QTY"].ToString();
                    //tda.Rejected = dtt.Rows[i]["REJECTED_QTY"].ToString();
                    //tda.exqty = dtt.Rows[i]["EXCEED_QTY"].ToString();
                    //tda.shortqty = dtt.Rows[i]["SHORT_QTY"].ToString();
                    //tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                    //tda.CF = dtt.Rows[i]["CF"].ToString();
                    //tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                    //tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    //tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    //tda.CGSTP = dtt.Rows[i]["CGSTP"].ToString();
                    //tda.SGSTP = dtt.Rows[i]["SGSTP"].ToString();
                    //tda.IGSTP = dtt.Rows[i]["IGSTP"].ToString();
                    //tda.CGST = dtt.Rows[i]["CGST"].ToString();
                    //tda.SGST = dtt.Rows[i]["SGST"].ToString();
                    //tda.IGST = dtt.Rows[i]["IGST"].ToString();
                    //tda.DiscPer = dtt.Rows[i]["DISC_PER"].ToString();
                    //tda.DiscAmount = dtt.Rows[i]["DIS_AMOUNT"].ToString();
                    //tda.Total = dtt.Rows[i]["TOTAL_AMOUNT"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            
                
            ic.GRNLst = TData;
            return View(ic);
        }

        private List<SelectListItem> BindGodownBin()
        {
            try
            {
                DataTable dtDesg = GRNService.GetGodownBin();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<SelectListItem> BindShopBin()
        {
            try
            {
                DataTable dtDesg = GRNService.GetShopBin();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListGRNgrid(string strStatus)
        {
            List<ListGRNgrid> Reg = new List<ListGRNgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = GRNService.GetAllListGRN(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                //string EditRow = string.Empty;
                string Accounts = string.Empty;
                string View = string.Empty;
                string Move = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    //EditRow = "<a href=Purchaseorder?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit 'width='20'  /></a>";
                    if (dtUsers.Rows[i]["PAYMENT_TAG"].ToString() == "0")
                    {
                        Accounts = "<a href=GRNAccount?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                        //DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                    }
                    View = "<a href=ViewGRN?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/file.png' alt='View' width='20'  /></a>";

                    Move = "<a href=MoveStock?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/sharing.png' alt='View Details' width='20' /></a>";
                    //DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "";

                }
                

                Reg.Add(new ListGRNgrid
                {
                    id = dtUsers.Rows[i]["GRN_BASIC_ID"].ToString(),
                    grn = dtUsers.Rows[i]["GRN_NO"].ToString(),
                    grndate = dtUsers.Rows[i]["GRN_DATE"].ToString(),
                    sup = dtUsers.Rows[i]["SUP_NAME"].ToString(),
                    net = dtUsers.Rows[i]["NET"].ToString(),
                    //editrow = EditRow,
                    accounts = Accounts,
                    view = View,
                    move = Move,

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = GRNService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListGRN");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListGRN");
            }
        }

        public List<SelectListItem> BindLedgerLst()
        {
            try
            {
                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetData("SELECT LEDGER_NAME,ID FROM ACC_LEDGER WHERE IS_ACTIVE='Y'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LEDGER_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GRNAccount(string id)
        {
            GRN grn = new GRN();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("SELECT NARRATION,AMTINWORDS,GRN_NO,GRN_DATE,SUP_NAME,REF_NO,REF_DATE,GROSS,NET,DISCOUNT,FRIGHTCHARGE,CGST,SGST,IGST,ROUNT_OFF FROM GRN_BASIC WHERE PAYMENT_TAG='0' AND GRN_BASIC_ID='" + id + "'");
            grn.GRNID = id;
            grn.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            grn.createdby = Request.Cookies["UserName"];
            //DataTable dt1 = new DataTable();
            //dt1 = datatrans.GetData("select SUM(DISC) DISC,SUM(CGST) CGST,SUM(SGST) SGST,SUM(IGST) IGST,SUM(TOTAMT) NET,SUM(IFREIGHTCH) FREIGHT from GRNBLDETAIL WHERE GRNBLBASICID='" + id + "'");
            //DataTable dtnat = datatrans.GetData("select I.ITEMID,BL.QTY,U.UNITID from GRNBLDETAIL BL,ITEMMASTER I,UNITMAST U where I.ITEMMASTERID=BL.ITEMID AND U.UNITMASTID=I.PRIUNIT AND GRNBLBASICID='" + id + "'");
            grn.Vmemo = dt.Rows[0]["NARRATION"].ToString();
            List<GRNAccount> TData = new List<GRNAccount>();
            GRNAccount tda = new GRNAccount();
            double totalcredit = 0;
            double totaldebit = 0;
            //DataTable dtdet = datatrans.GetData("select I.ITEMACC,SUM(AMOUNT) as GROSS from GRNBLDETAIL G,ITEMMASTER I  where G.ITEMID=I.ITEMMASTERID AND G.GRNBLBASICID='" + id + "' GROUP BY I.ITEMACC");
            DataTable dtacc = new DataTable();
            dtacc = datatrans.GetGRNconfig();
            string frieghtledger = "";
            string discledger = "";
            string roundoffledger = "";
            string cgstledger = "";
            string sgstledger = "";
            string igstledger = "";
            string packingledger = "";
            string grossledger = "";
            if (dtacc.Rows.Count > 0)
            {
                grn.ADCOMPHID = dtacc.Rows[0]["ADCOMPHID"].ToString();
                for (int i = 0; i < dtacc.Rows.Count; i++)
                {
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "FREIGHT")
                    {
                        frieghtledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "GROSS")
                    {
                        grossledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "DISCOUNT")
                    {
                        discledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString() == "ROUND OFF")
                    {
                        roundoffledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("CGST"))
                    {
                        cgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("SGST"))
                    {
                        sgstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                    if (dtacc.Rows[i]["ADTYPE"].ToString().Contains("IGST"))
                    {
                        igstledger = dtacc.Rows[i]["ADACCOUNT"].ToString();
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                grn.Round = Convert.ToDouble(dt.Rows[0]["ROUNT_OFF"].ToString() == "" ? "0" : dt.Rows[0]["ROUNT_OFF"].ToString());
                grn.Frieghtcharge = Convert.ToDouble(dt.Rows[0]["FRIGHTCHARGE"].ToString() == "" ? "0" : dt.Rows[0]["FRIGHTCHARGE"].ToString());
                grn.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                grn.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                grn.DiscAmt = Convert.ToDouble(dt.Rows[0]["DISCOUNT"].ToString() == "" ? "0" : dt.Rows[0]["DISCOUNT"].ToString());
                grn.CGST = Convert.ToDouble(dt.Rows[0]["CGST"].ToString() == "" ? "0" : dt.Rows[0]["CGST"].ToString());
                grn.SGST = Convert.ToDouble(dt.Rows[0]["SGST"].ToString() == "" ? "0" : dt.Rows[0]["SGST"].ToString());
                grn.IGST = Convert.ToDouble(dt.Rows[0]["IGST"].ToString() == "" ? "0" : dt.Rows[0]["IGST"].ToString());
             
               // DataTable dtParty = datatrans.GetData("select P.ACCOUNTNAME from GRNBLBASIC G,PARTYMAST P where G.PARTYID=P.PARTYMASTID AND G.GRNBLBASICID='" + id + "'");
                string mid = datatrans.GetDataString("select P.LEDGER_ID from GRN_BASIC G,SUPPLIER P where G.SUP_ID=P.ID AND G.GRN_BASIC_ID='" + id + "'");
                grn.mid = mid;
                if (grn.Net > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = mid;
                    tda.CRAmount = grn.Net;
                    tda.DRAmount = 0;
                    tda.TypeName = "NET";
                    tda.mid = mid;
                    tda.Isvalid = "Y";
                    tda.CRDR = "Cr";
                    tda.crdrh = "Cr";
                    tda.IsDisable = true;
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }

                if (grn.CGST > 0)
                {
                    tda = new GRNAccount();
                    //tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = cgstledger;
                    tda.mid = cgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.CGST;
                    tda.TypeName = "CGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    tda.IsDisable = true;
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.SGST > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = sgstledger;
                    tda.mid = sgstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.SGST;
                    tda.TypeName = "SGST";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    tda.IsDisable = true;
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.IGST > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = igstledger;
                    tda.mid = igstledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.IGST;
                    tda.TypeName = "IGST";
                    tda.Isvalid = "Y";
                    tda.IsDisable = true;
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.DiscAmt > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = discledger;
                    tda.mid = discledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = -grn.DiscAmt;
                    tda.TypeName = "Discount";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    tda.symbol = "+";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Packingcharges > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = packingledger;
                    tda.mid = packingledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Packingcharges;
                    tda.TypeName = "PACKING CHARGES";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    tda.symbol = "-";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    TData.Add(tda);
                }
                if (grn.Frieghtcharge > 0)
                {
                    tda = new GRNAccount();
                    //  tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = frieghtledger;
                    tda.mid = frieghtledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Frieghtcharge;
                    tda.TypeName = "FREIGHT CHARGES";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
              
                if (grn.Round > 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = roundoffledger;
                    tda.mid = roundoffledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Round;
                    tda.TypeName = "ROUND OFF";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "-";
                    TData.Add(tda);
                }
                if (grn.Round < 0)
                {
                    tda = new GRNAccount();
                    // tda.CRDRLst = BindCRDRLst();
                    tda.Ledgerlist = BindLedgerLst();
                    tda.Ledgername = roundoffledger;
                    tda.mid = roundoffledger;
                    tda.CRAmount = 0;
                    tda.DRAmount = grn.Round;
                    tda.TypeName = "ROUND OFF";
                    tda.Isvalid = "Y";
                    tda.CRDR = "Dr";
                    tda.crdrh = "Dr";
                    totalcredit += tda.CRAmount;
                    totaldebit += tda.DRAmount;
                    tda.symbol = "+";
                    TData.Add(tda);
                }
                if (grn.Gross > 0)
                {
                      tda = new GRNAccount();
                        tda.Ledgerlist = BindLedgerLst();
                        tda.Ledgername = grossledger;
                        tda.mid = grossledger;
                        tda.CRAmount = 0;
                        tda.DRAmount = grn.Gross;
                        tda.TypeName = "GROSS";
                        tda.Isvalid = "Y";
                        tda.CRDR = "Dr";
                        tda.crdrh = "Dr";
                        tda.symbol = "-";
                        tda.IsDisable = false;
                        totalcredit += tda.CRAmount;
                        totaldebit += tda.DRAmount;
                        TData.Add(tda);
                   

                }

            }
            grn.TotalCRAmt = Math.Round(totalcredit, 2);
            grn.TotalDRAmt = Math.Round(totaldebit, 2);
            grn.TotalNetmt = Math.Round(grn.Net, 2);
            grn.Acclst = TData;
            //grn.Accconfiglst = BindAccconfig();
            return View(grn);
        }

        [HttpPost]
        public ActionResult GRNAccount(GRN Cy, string id)
        {

            try
            {
                // Cy.GRNID = id;
                string Strout = GRNService.GRNACCOUNT(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.GRNID == null)
                    {
                        TempData["notice"] = "GRN Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "GRN Updated Successfully...!";
                    }
                    return RedirectToAction("ListGRN");
                }

                else
                {
                    ViewBag.PageTitle = "Edit GRN";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }


    }
}
