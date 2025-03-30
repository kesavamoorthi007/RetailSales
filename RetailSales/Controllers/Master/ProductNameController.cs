using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models.Master;
using RetailSales.Services.Master;

namespace RetailSales.Controllers.Master
{
    public class ProductNameController : Controller
    {
        IProductNameService ProductNameService;
        public ProductNameController(IProductNameService _ProductNameService)
        {
            ProductNameService = _ProductNameService;
        }
        public IActionResult ProductName(string id)
        {
            ProductName ic = new ProductName();
            ic.Categorylst = BindCategory();

            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();
                dt = ProductNameService.GetEditProductName(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Category = dt.Rows[0]["PRODUCT_CATEGORY"].ToString();
                    ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                    ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]

        public ActionResult ProductName(ProductName cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = ProductNameService.ProductNameCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Product Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Product Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductName");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Product";
                    TempData["notice"] = Strout;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public IActionResult ListProductName()
        {
            return View();
        }

        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = ProductNameService.GetCategory();
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

        public ActionResult MyListProductNamegrid(string strStatus)
        {
            List<ProsuctNamegrid> Reg = new List<ProsuctNamegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ProductNameService.GetAllProductNameGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Edit = "<a href=ProductName?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {

                    Edit = "";
                    Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new ProsuctNamegrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    category = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    proname = dtUsers.Rows[i]["PROD_NAME"].ToString(),
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

            string flag = ProductNameService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductName");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductName");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = ProductNameService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductName");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductName");
            }
        }
    }
}
