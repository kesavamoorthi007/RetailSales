using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Models.Accounts;
using RetailSales.Models;
using RetailSales.Services;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Accounts
{
    public class LedgersController : Controller
    {

        ILedgersServices LedgersService;
        public LedgersController(ILedgersServices _LedgersService)
        {
            LedgersService = _LedgersService;
        }

        public IActionResult Ledgers(string id)
        {

            Ledgers ic = new Ledgers();
            ic.AccountGroupList = BindAccountGroup();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = LedgersService.GetEditLedgersDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.AccountGroup = dt.Rows[0]["ACC_GRP_CODE"].ToString();
                    ic.LedgerName = dt.Rows[0]["LEDGER_NAME"].ToString();
                    ic.AllowZeroValue = dt.Rows[0]["ALLOW_ZERO_VAL"].ToString();
                    ic.TotalOpeningBalance = dt.Rows[0]["TOTAL_OPEN_BAL"].ToString();
                    ic.LedgerNotes = dt.Rows[0]["LDGR_NOTES"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Ledgers(Ledgers cy, string id)
        {
            try
            {
                cy.ID = id;
                string Strout = LedgersService.LedgersCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Ledgers Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Ledgers Updated Successfully...!";
                    }
                    return RedirectToAction("ListLedgers");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Ledgers";
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

        public List<SelectListItem> BindAccountGroup()
        {
            try
            {
                DataTable dtDesg = LedgersService.GetAccountName();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACC_GRP_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListLedgers()
        {
            return View();
        }

        public ActionResult MyListLedgersgrid(string strStatus)
        {
            List<ledgergrid> Reg = new List<ledgergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = LedgersService.GetAllLedgersGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Ledgers?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new ledgergrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    accountname = dtUsers.Rows[i]["ACC_GRP_NAME"].ToString(),
                    ledgername = dtUsers.Rows[i]["LEDGER_NAME"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = LedgersService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLedgers");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLedgers");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = LedgersService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLedgers");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLedgers");
            }
        }
    }
}
