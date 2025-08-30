using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Services;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class BankaccountsController : Controller
    {
        IBankaccountsService BankaccountsService;

        public BankaccountsController(IBankaccountsService _BankaccountsService)
        {
            BankaccountsService = _BankaccountsService;
        }

        public IActionResult Bankaccounts(string id)
        {
            Bankaccounts ic = new Bankaccounts();
            ic.Accounttypelst = BindAccounttype();
            ic.Countrylst = BindCountry();
            ic.Statelst = BindState();
            ic.Citylst = BindCity();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = BankaccountsService.GetEditBankaccountsDetail(id);
                if (dt.Rows.Count > 0)
                {

                    //ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Accounttypelst = BindAccounttype();
                    ic.Accounttype = dt.Rows[0]["ACC_TYPE"].ToString();
                    ic.Accountname = dt.Rows[0]["ACC_NAME"].ToString();
                    ic.Accountnumber = dt.Rows[0]["ACC_NO"].ToString();
                    ic.Bankname = dt.Rows[0]["BANK_NAME"].ToString();
                    ic.Branchname = dt.Rows[0]["BRANCH_NAME"].ToString();
                    ic.Branchaddress = dt.Rows[0]["BRANCH_ADDR"].ToString();
                    ic.Countrylst = BindCountry();
                    ic.Country = dt.Rows[0]["BR_COUNTRY"].ToString();
                    ic.Statelst = BindState();
                    ic.State = dt.Rows[0]["BR_STATE"].ToString();
                    ic.Citylst = BindCity();
                    ic.City = dt.Rows[0]["BR_CITY"].ToString();
                    ic.Bsrcode = dt.Rows[0]["BSR_CODE"].ToString();
                    ic.Ifsccode = dt.Rows[0]["IFSC_CODE"].ToString();
                    ic.ID = id;
                }


            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Bankaccounts(Bankaccounts Ic, string id)
        {
            ViewBag.PageTitle = "Bankaccounts";
            try
            {
                Ic.ID = id;
                string Strout = BankaccountsService.BankaccountsCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Bankaccounts Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Bankaccounts Updated Successfully...!";
                    }
                    return RedirectToAction("ListBankaccounts");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Bankaccounts";
                    TempData["notice"] = Strout;
                    return RedirectToAction("Bankaccounts");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }
        public IActionResult ListBankaccounts()
        {
            return View();
        }

        public List<SelectListItem> BindAccounttype()
        {
            try
            {
                DataTable dtDesg = BankaccountsService.GetAccounttype();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACC_TYPE_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = BankaccountsService.GetCountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = BankaccountsService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindCity()
        {
            try
            {
                DataTable dtDesg = BankaccountsService.GetCity();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListBankaccountsgrid(string strStatus)
        {
            List<Bankaccountsgrid> Reg = new List<Bankaccountsgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = BankaccountsService.GetAllBankaccountsGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Bankaccounts?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new Bankaccountsgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    
                    accname = dtUsers.Rows[i]["ACC_NAME"].ToString(),
                    bname = dtUsers.Rows[i]["BANK_NAME"].ToString(),
                    accnum = dtUsers.Rows[i]["ACC_NO"].ToString(),
                  
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
            string flag = "";
            if (tag == "Del")
            {
                flag = BankaccountsService.StatusChange(tag, id);
            }
            else
            {
                flag = BankaccountsService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListBankaccounts");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListBankaccounts");
            }
        }
        //public ActionResult Remove(string tag, string id)
        //{

        //    string flag = BankaccountsService.RemoveChange(tag, id);
        //    if (string.IsNullOrEmpty(flag))
        //    {

        //        return RedirectToAction("ListBankaccounts");
        //    }
        //    else
        //    {
        //        TempData["notice"] = flag;
        //        return RedirectToAction("ListBankaccounts");
        //    }
        //}

    }
}

