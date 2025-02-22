using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Models.Inventory;
using RetailSales.Services.Accounts;
using RetailSales.Services.Inventory;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class DebitNoteController : Controller
    {
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        IDebitNoteService DebitNoteService;
        public DebitNoteController(IDebitNoteService _DebitNoteService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            DebitNoteService = _DebitNoteService;
        }
              
        public IActionResult DebitNote(string id)
        {
            DebitNote ic = new DebitNote();

            DebitNoteItem tda = new DebitNoteItem();
            List<DebitNoteItem> TData = new List<DebitNoteItem>();                                 
            ic.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("PurchaseOrder");
            if (dtv.Rows.Count > 0)
            {
                ic.VocNo = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {
                for (int i = 0; i < 2; i++)
                {
                    tda = new DebitNoteItem();
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
            ic.DebitNotelst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult DebitNote(DebitNote cy, string id)
            {

            try
            {
                cy.ID = id;
                string Strout = DebitNoteService.DebitNoteCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "DebitNote Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DebitNote Updated Successfully...!";
                    }
                    return RedirectToAction("DebitNote");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DebitNote";
                    TempData["notice"] = Strout;                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public ActionResult GetLedgerDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string balance = "";
                dt = DebitNoteService.GetLedgerDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    balance = dt.Rows[0]["CLOSE_BAL"].ToString();
                }

                var result = new { balance = balance };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetDebJSON()
        {
            DebitNoteItem model = new DebitNoteItem();
            model.DBCRlst = BindDbCr();
            return Json(BindDbCr());

        }
        public JsonResult GetAccJSON()
        {
            DebitNoteItem model = new DebitNoteItem();
            return Json(BindAcc(""));

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
                DataTable dtDesg = DebitNoteService.GetAcc(id);
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

        public ActionResult GetLedgerDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string balance = "";
                dt = DebitNoteService.GetLedgerDetails(ItemId);
                
                if (dt.Rows.Count > 0)
                {
                    balance = dt.Rows[0]["CLOSE_BAL"].ToString();
                }

                var result = new { balance = balance };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
