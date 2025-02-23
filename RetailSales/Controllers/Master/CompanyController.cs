using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class CompanyController : Controller
    {
        ICompanyService CompanyService;
        public CompanyController(ICompanyService _CompanyService)
        {
            CompanyService = _CompanyService;
        }
        public IActionResult Company(string id)
        {
            Company ic = new Company();
            ic.Countrylst = BindCountry();
            ic.Statelst = BindState();
            ic.Citylst = BindCity();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = CompanyService.GetEditCompanyDetail(id);
                if (dt.Rows.Count > 0)
                {
                    
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.CompanyName = dt.Rows[0]["COMPANY_NAME"].ToString();
                    ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ic.Mobile = dt.Rows[0]["MOBILE_NO"].ToString();
                    ic.Countrylst = BindCountry();
                    ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                    ic.Statelst = BindState();
                    ic.State = dt.Rows[0]["STATE"].ToString();
                    ic.Citylst = BindCity();
                    ic.City = dt.Rows[0]["CITY"].ToString();
                    ic.Email = dt.Rows[0]["EMAIL_ID"].ToString();
                    ic.Landline = dt.Rows[0]["LAND_LINE"].ToString();
                    ic.Website = dt.Rows[0]["WEBSITE"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Company(Company cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = CompanyService.CompanyCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Company Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Company Updated Successfully...!";
                    }
                    return RedirectToAction("ListCompany");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Company";
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
        public IActionResult ListCompany()
        {
            return View();
        }
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = CompanyService.GetCountry();
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
                DataTable dtDesg = CompanyService.GetState();
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
                DataTable dtDesg = CompanyService.GetCity();
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
        public ActionResult MyListCompanygrid(string strStatus)
        {
            List<Companygrid> Reg = new List<Companygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CompanyService.GetAllCompanyGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Company?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Companygrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    coname = dtUsers.Rows[i]["COMPANY_NAME"].ToString(),
                    address = dtUsers.Rows[i]["ADDRESS"].ToString(),
                    city = dtUsers.Rows[i]["CITY_NAME"].ToString(),
                    state = dtUsers.Rows[i]["STATE_NAME"].ToString(),
                    country = dtUsers.Rows[i]["COUNTRY_NAME"].ToString(),
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

            string flag = CompanyService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCompany");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCompany");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = CompanyService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCompany");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCompany");
            }
        }
    }
}
