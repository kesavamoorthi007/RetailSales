using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Services;
using RetailSales.Services.Accounts;
using RetailSales.Services.Master;
using System.Data;
using System.Data.SqlClient;
//using AccountGroup = RetailSales.Models.Accounts.AccountGroup;

namespace RetailSales.Controllers.Accounts
{
    public class AccountGroupController : Controller
    {

        IAccountGroupService AccountGroupService;
        public AccountGroupController(IAccountGroupService _AccountGroupService)
        {
            AccountGroupService = _AccountGroupService;
        }

        public IActionResult AccountGroup(string id)
        {
            AccountGroup ic = new AccountGroup();

            // Account class binding
            ic.accclasslist = BindAccClass();
            // Account type binding
            ic.acctypelist = BindAccType();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = AccountGroupService.GetEditAccountGroupDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.AccountClass = dt.Rows[0]["ACC_CLASS"].ToString();
                    ic.AccountType = dt.Rows[0]["ACC_TYPE_CODE"].ToString();
                    ic.AccountGroupName = dt.Rows[0]["ACC_GRP_NAME"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]

        public ActionResult AccountGroup(AccountGroup cy, string id)
        {

            try
             {
                cy.ID = id;
                string Strout = AccountGroupService.AccountGroupCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Account Group Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Account Group Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccountGroup");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccountGroup";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        // Binding Account class
        public List<SelectListItem> BindAccClass()
        {
            try
            {
                DataTable dtDesg = AccountGroupService.GetAccountClass();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACC_CLASS"].ToString(), Value = dtDesg.Rows[i]["ACC_CLASS"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Binding Account Type
        public List<SelectListItem> BindAccType()
        {
            try
            {
                DataTable dtDesg1 = AccountGroupService.GetAccountType();
                List<SelectListItem> lstdesg1 = new List<SelectListItem>();
                for (int i = 0; i < dtDesg1.Rows.Count; i++)
                {
                    lstdesg1.Add(new SelectListItem() { Text = dtDesg1.Rows[i]["ACC_TYPE_NAME"].ToString(), Value = dtDesg1.Rows[i]["ACC_TYPE_CODE"].ToString() });
                }
                return lstdesg1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListAccountGroupgrid(string strStatus)
        {
            List<ListAccountGroupgrid> Reg = new List<ListAccountGroupgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = AccountGroupService.GetAllAccountGroupGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    Edit = "<a href=AccountGroup?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    Edit = "";
                    Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new ListAccountGroupgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    accclass = dtUsers.Rows[i]["ACC_CLASS"].ToString(),
                    acctype = dtUsers.Rows[i]["ACC_TYPE_NAME"].ToString(),
                    accgrpname = dtUsers.Rows[i]["ACC_GRP_NAME"].ToString(),
                    edit = Edit,
                    delete = Delete,

                });
            }

            return Json(new
            {
                Reg
            });

        }


        public IActionResult ListAccountGroup()
        {
            return View();
        }

        public IActionResult Daybook()
        {
            return View();
        }

        public ActionResult MyListDayBookgrid(string strfrom, string strTo ,string strStatus)
        {
            List<ListDayItems> Reg = new List<ListDayItems>();
            DataTable dtUsers = new DataTable();
            dtUsers = AccountGroupService.GetDaydet(strfrom, strTo, strStatus);
            DataTable dt = new DataTable();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                // dt = (DataTable)ledger.GetAllListDayBookItems(dtUsers.Rows[i]["MID"].ToString());

                Reg.Add(new ListDayItems
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    vocherno = dtUsers.Rows[i]["VOUCH_NO"].ToString(),
                    vocherdate = dtUsers.Rows[i]["VOUCH_DATE"].ToString(),
                    tratype = dtUsers.Rows[i]["REF_TYPE"].ToString(),
                    vocmemo = dtUsers.Rows[i]["VOUCH_MEMO"].ToString(),
                    vtype = dtUsers.Rows[i]["MID"].ToString(),
                    type = dtUsers.Rows[i]["TRANS_TYPE"].ToString(),
                    ledgercode = dtUsers.Rows[i]["ledger"].ToString(),
                    debitamount = dtUsers.Rows[i]["DBAMOUNT"].ToString(),
                    creditamount = dtUsers.Rows[i]["CRAMOUNT"].ToString(),
                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = AccountGroupService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccountGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccountGroup");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = AccountGroupService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccountGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccountGroup");
            }
        }

    }
}
