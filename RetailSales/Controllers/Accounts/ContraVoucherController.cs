using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Services.Accounts;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class ContraVoucherController : Controller
    {
        IContraVoucherService ContraVoucherService;
        public ContraVoucherController(IContraVoucherService _ContraVoucherService)
        {
            ContraVoucherService = _ContraVoucherService;
        }
        public IActionResult ContraVoucher(string id)
        {
            ContraVoucher ca = new ContraVoucher();

            ca.ExcRate = "1";
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.VocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Chedate = DateTime.Now.ToString("dd-MMM-yyyy");

            Contra pr = new Contra();
            List<Contra> TData = new List<Contra>();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    pr = new Contra();
                    pr.Isvalid = "Y";
                    pr.DBCRlst = BindDbCr();
                    pr.AccNamelst = BindAcc();
                    pr.DBCR = "Dr";
                    TData.Add(pr);
                }
            }
            ca.Contralst = TData;
            return View(ca);
        }

        public ActionResult GetLedgerDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string balance = "";
                dt = ContraVoucherService.GetLedgerDetails(ItemId);

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
                DataTable dtDesg = ContraVoucherService.GetAcc();
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
            Contra model = new Contra();
            return Json(BindDbCr());

        }
        public JsonResult GetAccJSON()
        {
            Contra model = new Contra();
            return Json(BindAcc());

        }
    }
}
