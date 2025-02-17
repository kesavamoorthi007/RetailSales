using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Services.Accounts;
using RetailSales.Services.Purchase;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class CreditNoteController : Controller
    {
        ICreditNoteService CreditNoteService;
        public CreditNoteController(ICreditNoteService _CreditNoteService)
        {
            CreditNoteService = _CreditNoteService;
        }
        public IActionResult CreditNote(string id)
        {
            CreditNote ic = new CreditNote();
            ic.ExcRate = "1";
            ic.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");

            CreditNoteItem tda = new CreditNoteItem();
            List<CreditNoteItem> TData = new List<CreditNoteItem>();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new CreditNoteItem();
                    tda.Isvalid = "Y";
                    tda.DBCRlst = BindDbCr();
                    tda.AccNamelst = BindAcc();
                    tda.DBCR = "Dr";
                    TData.Add(tda);
                }
            }
            else
            {

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
        public List<SelectListItem> BindAcc()
        {
            try
            {
                DataTable dtDesg = CreditNoteService.GetAcc();
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
        public JsonResult GetDebJSON()
        {
            CreditNoteItem model = new CreditNoteItem();
            return Json(BindDbCr());

        }
        public JsonResult GetAccJSON()
        {
            CreditNoteItem model = new CreditNoteItem();
            return Json(BindAcc());

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
                    return RedirectToAction("ListPurchaseorder");
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
    }
}
