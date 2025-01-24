using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class DebitNoteController : Controller
    {
        IDebitNoteService DebitNoteService;
        public DebitNoteController(IDebitNoteService _DebitNoteService)
        {
            DebitNoteService = _DebitNoteService;
        }
        public IActionResult DebitNote(string id)
        {
            DebitNote ic = new DebitNote();

            DebitNoteItem tda = new DebitNoteItem();
            List<DebitNoteItem> TData = new List<DebitNoteItem>();

            ic.ExcRate = "1";
            ic.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new DebitNoteItem();
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
            ic.DebitNotelst = TData;
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
                DataTable dtDesg = DebitNoteService.GetAcc();
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
            DebitNoteItem model = new DebitNoteItem();
            return Json(BindDbCr());

        }
        public JsonResult GetAccJSON()
        {
            DebitNoteItem model = new DebitNoteItem();
            return Json(BindAcc());

        }
    }
}
