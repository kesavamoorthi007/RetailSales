using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class ProductController : Controller
    {
        IProductService ProductService;
        public ProductController(IProductService _ProductService)
        {
            ProductService = _ProductService;
        }
        public IActionResult Product(string id)
        {
            Product ic = new Product();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = ProductService.GetEditProductDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.ProductName = dt.Rows[0]["PRODUCT_NAME"].ToString();
                    
                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Product(Product cy, string id)
        {
            ViewBag.PageTitle = "Product";
            try
            {
                cy.ID = id;
                string Strout = ProductService.ProductCRUD(cy);
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
                    return RedirectToAction("ListProduct");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Product";
                    TempData["notice"] = Strout;
                    return RedirectToAction("Product");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public IActionResult ListProduct()
        {
            return View();
        }

        public ActionResult MyListProductgrid(string strStatus)
        {
            List<Productgrid> Reg = new List<Productgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ProductService.GetAllProductGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Product?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new Productgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
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
                flag = ProductService.StatusChange(tag, id);
            }
            else
            {
                flag = ProductService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProduct");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProduct");
            }
        }
    }
}
