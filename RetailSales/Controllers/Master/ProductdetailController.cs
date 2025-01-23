using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
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

    }
}

