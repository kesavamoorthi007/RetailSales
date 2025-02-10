using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Inventory;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using RetailSales.Models.Inventory;
using RetailSales.Services.Sales;

namespace RetailSales.Controllers.Inventory
{
    public class StockAdjustmentController : Controller
    {
        IStockAdjustmentService StockAdjustmentService;
        public StockAdjustmentController(IStockAdjustmentService _StockAdjustmentService)
        {
            StockAdjustmentService = _StockAdjustmentService;
        }
        public IActionResult StockAdjustment(string id)
        {
            StockAdjustment ic = new StockAdjustment();
            ic.Type = "Addition";
            ic.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");

            List<StockAdjustmentItem> TData = new List<StockAdjustmentItem>();
            StockAdjustmentItem tda = new StockAdjustmentItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new StockAdjustmentItem();
                    tda.Itemlst = BindItem();
                    tda.Variantlst = BindVariant("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {


            }
            ic.StockAdjustmentList = TData;
            return View(ic);
        }

        private List<SelectListItem> BindVariant(string id)
        {
            try
            {
                DataTable dtDesg = StockAdjustmentService.GetVariant(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_VARIANT"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]

        public JsonResult GetItemGrpJSON()
        {
            StockAdjustmentItem model = new StockAdjustmentItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }

        public JsonResult GetVarientJSON(string id)
        {
            //StockAdjustmentItem model = new StockAdjustmentItem();
            //model.ItemVariantlst = BindItemVariant();
            return Json(BindVariant(id));
        }

        private List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = StockAdjustmentService.GetItem();
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
                string unit = "";
                string rate = "";
                dt = StockAdjustmentService.GetVariantDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UOM_CODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();


                }

                var result = new { unit = unit, rate = rate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
