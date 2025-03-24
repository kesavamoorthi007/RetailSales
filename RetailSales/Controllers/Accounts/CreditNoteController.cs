using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Services.Accounts;
using RetailSales.Services.Inventory;
using RetailSales.Services.Purchase;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class CreditNoteController : Controller
    {
        
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        ICreditNoteService CreditNoteService;
        public CreditNoteController(ICreditNoteService _CreditNoteService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            CreditNoteService = _CreditNoteService;
        }

        
       
        public IActionResult CreditNote(string id)
        {
            CreditNote ic = new CreditNote();                       
            ic.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Credit Note");
            if (dtv.Rows.Count > 0)
            {
                ic.VocNo = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }
            CreditNoteItem tda = new CreditNoteItem();
            List<CreditNoteItem> TData = new List<CreditNoteItem>();
            if (id == null)
            {
                for (int i = 0; i < 2; i++)
                {
                    tda = new CreditNoteItem();
                    tda.Isvalid = "Y";
                    tda.DBCRlst = BindDbCr();
                    tda.AccNamelst = BindAcc("");                    
;                   tda.DBCR = "Dr";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt = CreditNoteService.GetEditCreditNote(id);
                if (dt.Rows.Count > 0)
                {
                    ic.VocNo = dt.Rows[0]["PONO"].ToString();
                    ic.VocDate = dt.Rows[0]["PODATE"].ToString();
                    //ic.Suplst = BindSupplier();
                    ic.exchangeRate = dt.Rows[0]["SUP_NAME"].ToString();
                    ic.RefNo = dt.Rows[0]["ADDRESS"].ToString();
                    ic.RefDate = dt.Rows[0]["COUNTRY"].ToString();
                    ic.Currency = dt.Rows[0]["STATE"].ToString();
                    ic.totdeb = dt.Rows[0]["CITY"].ToString();
                    ic.totcri = dt.Rows[0]["REF_NO"].ToString();
                    ic.totamt = dt.Rows[0]["REF_DATE"].ToString();
                    ic.AmtWd = dt.Rows[0]["AMTINWORDS"].ToString();
                    ic.Narr = dt.Rows[0]["NARRATION"].ToString();                
                    ic.ID = id;

                }
                DataTable dtt = new DataTable();
                dtt = CreditNoteService.GetCreditNoteDetailes(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new CreditNoteItem();
                        //tda.Itemlst = BindItem();
                        tda.DBCR = dtt.Rows[i]["ITEM"].ToString();
                        //tda.AccName = BindVarient(tda.Item);
                        tda.AccName = dtt.Rows[i]["VARIANT"].ToString();                       
                        tda.DebitAmt = Convert.ToDouble(dtt.Rows[i]["TARIFF"]);
                        tda.CreditAmt = Convert.ToDouble(dtt.Rows[i]["TARIFF"]);
                        tda.Balance = dtt.Rows[i]["QTY"].ToString();                      
                        tda.ID = id;
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            ic.CreditNotelst = TData;
            return View(ic);
        }
        public List<SelectListItem> BindDbCr()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Dr", Value = "Dr" });
                lstdesg.Add(new SelectListItem() { Text = "Cr", Value = "Cr" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<SelectListItem> BindAcc(string id)
        {
            try
            {
                DataTable dtDesg = CreditNoteService.GetAcc(id);
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

        public ActionResult GetLedgerDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string balance = "";               
                dt = CreditNoteService.GetLedgerDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    balance = dt.Rows[0]["CLOSE_BAL"].ToString();
                }

                var result = new { balance = balance};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult GetDebJSON()
        {
            CreditNoteItem model = new CreditNoteItem();
            return Json(BindDbCr());

        }
        public JsonResult GetAccJSON(string id)
        {
            CreditNoteItem model = new CreditNoteItem();
            return Json(BindAcc(id));
        }
        [HttpPost]
        public ActionResult CreditNote(CreditNote cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = CreditNoteService.CreditNoteCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "CreditNote Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CreditNote Updated Successfully...!";
                    }
                    return RedirectToAction("CreditNote");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CreditNote";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public IActionResult SalesCreditNote(string id)
        {
            SalesCreditNote ic = new SalesCreditNote();
            ic.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.InvDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Credit Note");
            if (dtv.Rows.Count > 0)
            {
                ic.VocNo = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
                ic.InvNo = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }
            SalesCreditNoteItem tda = new SalesCreditNoteItem();
            List<SalesCreditNoteItem> TData = new List<SalesCreditNoteItem>();
            if (id == null)
            {
                for (int i = 0; i < 2; i++)
                {
                    tda = new SalesCreditNoteItem();
                    tda.Isvalid = "Y";
                    tda.DBCRlst = BindDbCr();
                    tda.AccNamelst = BindAcc("");
                    tda.DBCR = "Dr";
                    TData.Add(tda);
                }
            }
            else
            {
                
            }
            ic.SalesCreditNotelst = TData;
            return View(ic);
        }

        [HttpPost]
        public ActionResult SalesCreditNote(CreditNote cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = CreditNoteService.CreditNoteCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "SalesCreditNote Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesCreditNote Updated Successfully...!";
                    }
                    return RedirectToAction("SalesCreditNote");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesCreditNote";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

    }
}
