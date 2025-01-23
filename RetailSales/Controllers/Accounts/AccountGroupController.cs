using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface;
using RetailSales.Interface.Accounts;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Services;
using RetailSales.Services.Accounts;
using System.Data;
using AccountGroup = RetailSales.Models.Accounts.AccountGroup;

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

        // Binding Account class
        public List<SelectListItem> BindAccClass()
        {
            try
            {
                DataTable dtDesg = AccountGroupService.GetAccountClass();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACC_CLASS"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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
                    lstdesg1.Add(new SelectListItem() { Text = dtDesg1.Rows[i]["ACC_TYPE_CODE"].ToString(), Value = dtDesg1.Rows[i]["ID"].ToString() });
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
                    Edit = "<a><img src='../Images/edit.png' alt='Edit' width='20' /></a>";
                    Delete = "<a><img src='../Images/Inactive.png' alt='Deactivate' width='20' /></a>";
                }
                else
                {
                    Edit = "";
                    Delete = "<a><img src='../Images/reactive.png' alt='Reactive' width='20' /></a>";
                }
                Reg.Add(new ListAccountGroupgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    accclass = dtUsers.Rows[i]["ACC_CLASS"].ToString(),
                    acctype = dtUsers.Rows[i]["ACC_TYPE_CODE"].ToString(),
                    accgrpname = dtUsers.Rows[i]["ACC_GRP_NAME"].ToString(),
                    //edit = Edit,
                    //delete = Delete,

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
    }
}
