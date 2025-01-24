using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
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

            if (id == null)
            {

            }
            else
            {


            }
            return View(ic);
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
        public IActionResult ListProductdetail()
        {
            return View();
        }
        public ActionResult MyListProductdetailgrid()
        {
            List<Productdetailgrid> Reg = new List<Productdetailgrid>();
            DataTable dtUsers = new DataTable();
            dtUsers = ProductdetailService.GetAllProductDeatilsGRID();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                EditRow = "<a><img src='../Images/edit.png' alt='Edit'  /></a>";
                DeleteRow = "<a><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                Reg.Add(new Productdetailgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    varint = dtUsers.Rows[i]["VARIANT"].ToString(),
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

    }
}

