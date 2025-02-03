using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class ProductdetailController : Controller
    {
        IProductdetailService  ProductdetailService;

        public ProductdetailController(IProductdetailService _ProductdetailService)
        {
            ProductdetailService = _ProductdetailService;
        }
        public IActionResult Productdetail(string id)
        {
            Productdetail ic = new Productdetail();

            ic.Categorylst = BindCategory();
            ic.Uomlst = BindUOM();
            ic.Hsnlst = BindHsn();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = ProductdetailService.GetEditProductdetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Product = dt.Rows[0]["PRODUCT_CATEGORY"].ToString();
                    ic.Varint = dt.Rows[0]["PRODUCT_VARIANT"].ToString();
                    ic.Varintnic = dt.Rows[0]["VARIANT_NICKNAME"].ToString();
                    ic.Productdescription = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
                    ic.Hsncode = dt.Rows[0]["HSN_CODE"].ToString();
                    ic.Uom = dt.Rows[0]["UOM"].ToString();
                    ic.Rate = dt.Rows[0]["RATE"].ToString();
                    

                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Productdetail(Productdetail cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = ProductdetailService.ProductdetailCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Productdetail Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Productdetail Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductdetail");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Productdetail";
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
        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetUom();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindHsn()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetHsn();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["HSCODE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListProductdetail()
        {
            return View();
        }
        public ActionResult MyListProductdetailgrid(string strStatus)
        {
            List<Productdetailgrid> Reg = new List<Productdetailgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ProductdetailService.GetAllProductDeatilsGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Productdetail?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Productdetailgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT_CATEGORY"].ToString(),
                    varint = dtUsers.Rows[i]["PRODUCT_VARIANT"].ToString(),
                    varintnic = dtUsers.Rows[i]["VARIANT_NICKNAME"].ToString(),
                    uom = dtUsers.Rows[i]["UOM"].ToString(),
                    rate = dtUsers.Rows[i]["RATE"].ToString(),
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

                string flag = ProductdetailService.StatusChange(tag, id);
                if (string.IsNullOrEmpty(flag))
                {

                    return RedirectToAction("ListProductdetail");
                }
                else
                {
                    TempData["notice"] = flag;
                    return RedirectToAction("ListProductdetail");
                }
            }
            public ActionResult Remove(string tag, string id)
            {

                string flag = ProductdetailService.RemoveChange(tag, id);
                if (string.IsNullOrEmpty(flag))
                {

                    return RedirectToAction("ListProductdetail");
                }
                else
                {
                    TempData["notice"] = flag;
                    return RedirectToAction("ListProductdetail");
                }
            }

       

    }
}

