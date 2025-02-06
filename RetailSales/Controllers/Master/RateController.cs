using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using RetailSales.Services.Sales;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class RateController : Controller
    {
        IRateService RateService;
        public RateController(IRateService _RateService)
        {
            RateService = _RateService;
        }
        public IActionResult Rate(string id)
        {
            Rate ic = new Rate();
            ic.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");

            List<RateList> TData = new List<RateList>();
            RateList tda = new RateList();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new RateList();
                    tda.Itemlst = BindItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {


            }
            ic.RateList = TData;
            return View(ic);
        }

        public JsonResult GetItemGrpJSON()
        {
            RateList model = new RateList();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }

        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = RateService.GetItem();
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

        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                //string item = "";
                string unit = "";
                string rate = "";
                dt = RateService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    //item = dt.Rows[0]["VARIANT"].ToString();
                    unit = dt.Rows[0]["UOM"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();


                }
                //item = item,
                var result = new {unit = unit, rate = rate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }    
}
