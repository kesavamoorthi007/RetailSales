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


            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Bankaccounts(Bankaccounts Ic, string id)
        {

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
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
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
        //public ActionResult MyListBankaccountsServicegrid(string strStatus)
        //{
        //    List<Bankaccountsgrid> Reg = new List<Bankaccountsgrid>();
        //    DataTable dtUsers = new DataTable();
        //    strStatus = strStatus == "" ? "Y" : strStatus;
        //    dtUsers = BankaccountsService.GetAllBankaccountsGRID(strStatus);
        //    for (int i = 0; i < dtUsers.Rows.Count; i++)
        //    {

        //        string DeleteRow = string.Empty;
        //        string EditRow = string.Empty;

        //        if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
        //        {
        //            EditRow = "<a href=Bankaccounts?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
        //            DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
        //        }
        //        else
        //        {
        //            EditRow = "";
        //            DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
        //        }
        //        Reg.Add(new Customergrid
        //        {
        //            id = dtUsers.Rows[i]["ID"].ToString(),
        //            accname = dtUsers.Rows[i]["ACC_NAME"].ToString(),
        //            bname = dtUsers.Rows[i]["BANK_NAME"].ToString(),
        //            acctype = dtUsers.Rows[i]["ACC_TYPE"].ToString(),
        //            branch = dtUsers.Rows[i]["BRANCH_NAME"].ToString(),
        //            badd = dtUsers.Rows[i]["BRANCH_ADDR"].ToString(),
        //            country = dtUsers.Rows[i]["BR_COUNTRY"].ToString(),
        //            state = dtUsers.Rows[i]["BR_STATE"].ToString(),
        //            city = dtUsers.Rows[i]["BR_CITY"].ToString(),
        //            code = dtUsers.Rows[i]["BSR_CODE"].ToString(),
        //            ifsc = dtUsers.Rows[i]["IFSC_CODE"].ToString(),
        //            isdefault = dtUsers.Rows[i]["IS_DEFAULT"].ToString(),
        //            editrow = EditRow,
        //            delrow = DeleteRow,

        //        });
        //    }

        //    return Json(new
        //    {
        //        Reg
        //    });

        //}
       
    }
}

