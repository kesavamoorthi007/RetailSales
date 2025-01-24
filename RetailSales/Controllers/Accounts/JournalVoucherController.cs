using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Services.Accounts;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class JournalVoucherController : Controller
    {
        IJournalVoucherService JournalVoucherService;
        public JournalVoucherController(IJournalVoucherService _JournalVoucherService)
        {
            JournalVoucherService = _JournalVoucherService;
        }
        public IActionResult JournalVoucher(string id)
        {
            JournalVoucher ic = new JournalVoucher();

            JournalVoucherItem tda = new JournalVoucherItem();
            List<JournalVoucherItem> TData = new List<JournalVoucherItem>();

            ic.SecIDLst = BindSection();
            ic.ExcRate = "1";
            ic.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new JournalVoucherItem();
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
            ic.JournalVoucherlst = TData;
            return View(ic);
        }
        public List<SelectListItem> BindSection()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "COMMISSION", Value = "COMMISSION" });
                lstdesg.Add(new SelectListItem() { Text = "CONTRACT", Value = "CONTRACT" });
                lstdesg.Add(new SelectListItem() { Text = "INTEREST", Value = "INTEREST" });
                lstdesg.Add(new SelectListItem() { Text = "PROFESSIONAL", Value = "PROFESSIONAL" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                DataTable dtDesg = JournalVoucherService.GetAcc();
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
            JournalVoucherItem model = new JournalVoucherItem();
            return Json(BindDbCr());

        }
        public JsonResult GetAccJSON()
        {
            JournalVoucherItem model = new JournalVoucherItem();
            return Json(BindAcc());

        }
    }
}
