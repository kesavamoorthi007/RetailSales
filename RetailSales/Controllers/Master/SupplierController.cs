using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Models.Master;
//using Supplier = RetailSales.Models.Supplier;
using RetailSales.Services.Accounts;
using RetailSales.Models.Accounts;
using RetailSales.Services.Master;

namespace RetailSales.Controllers.Master
{
    public class SupplierController : Controller
    {
        ISupplierService SupplierService;
        public SupplierController(ISupplierService _SupplierService)
        {
            SupplierService = _SupplierService;
        }

        public IActionResult Supplier(String id)
        {
            Supplier ic = new Supplier();
            
            ic.Statelst = BindState();
            ic.Citylst = BindCity("");
            ic.Categorylst = BindCategory();
            
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = SupplierService.GetEditSupplier(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Supp = dt.Rows[0]["SUPPLIER_NAME"].ToString();
                    ic.Category = dt.Rows[0]["SUPP_CAT"].ToString();
                    //ic.Delivery = dt.Rows[0]["DEL_LEAD_TIME"].ToString();
                    ic.Days = dt.Rows[0]["CR_DAYS"].ToString();
                 
                    ic.State = dt.Rows[0]["STATE"].ToString();
                    ic.Citylst = BindCity(ic.State);
                    ic.City = dt.Rows[0]["CITY"].ToString();
                    //ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                    ic.Gst = dt.Rows[0]["GST_NO"].ToString();
                    ic.Mobile = dt.Rows[0]["MOBILE_NO"].ToString();
                    ic.Landline = dt.Rows[0]["LANDLINE_NO"].ToString();
                    ic.Email = dt.Rows[0]["EMAIL_ID"].ToString();
                    ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Supplier(Supplier cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = SupplierService.SupplierCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Supplier Details Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Supplier Details Updated Successfully...!";
                    }
                    return RedirectToAction("ListSupplier");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Supplier";
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

        public IActionResult ListSupplier()
        {
            return View();
        }
        public JsonResult GetCityJSON(string cityid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindCity(cityid));
        }
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = SupplierService.GetState();
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
        public List<SelectListItem> BindCity(string cityid)
        {
            try
            {
                DataTable dtDesg = SupplierService.GetCity(cityid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITY_NAME"].ToString(), Value = dtDesg.Rows[i]["CITY_NAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = SupplierService.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CATGRY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListSuppliergrid(string strStatus)
        {
            List<ListSuppliergrid> Reg = new List<ListSuppliergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SupplierService.GetAllSupplierGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Edit = "<a href=Supplier?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {                   

                    Edit = "";
                    Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new ListSuppliergrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    supp = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    category = dtUsers.Rows[i]["CATGRY_NAME"].ToString(),
                    days = dtUsers.Rows[i]["CR_DAYS"].ToString(),
                    edit = Edit,
                    delete = Delete,

                });
            }

            return Json(new
            {
                Reg
            });

        }


        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = SupplierService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSupplier");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSupplier");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = SupplierService.RemoveChange( tag,  id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSupplier");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSupplier");
            }
        }

        
    }
}
