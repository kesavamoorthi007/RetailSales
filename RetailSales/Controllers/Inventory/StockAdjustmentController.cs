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
                    tda.ItemVariantlst = BindItemVariant();
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

        public JsonResult GetItemGrpJSON()
        {
            StockAdjustmentItem model = new StockAdjustmentItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }

        public JsonResult GetItemVariantGrpJSON()
        {
            StockAdjustmentItem model = new StockAdjustmentItem();
            model.ItemVariantlst = BindItemVariant();
            return Json(BindItemVariant());
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

        private List<SelectListItem> BindItemVariant()
        {
            try
            {
                DataTable dtDesg = StockAdjustmentService.GetItemVariant();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["VARIANT"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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
                string stockqty = "";                
                string rate = "";
                dt = StockAdjustmentService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UOM"].ToString();
                    stockqty = dt.Rows[0]["BALANCE_QUANTITY"].ToString();
                    rate = dt.Rows[0]["UNIT_COST"].ToString();


                }

                var result = new { unit = unit, stockqty = stockqty, rate = rate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
