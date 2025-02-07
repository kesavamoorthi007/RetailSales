using Microsoft.AspNetCore.Mvc;
using RetailSales.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Services;


namespace RetailSales.Controllers
{
    public class StockTransferController : Controller
    {
        IStockTransferService StockTransferService;

        private object grid;

        public StockTransferController(IStockTransferService _StockTransferService)
        {
            StockTransferService = _StockTransferService;
        }
        public IActionResult StockTransfer(string id)
        {
            StockTransfer ic = new StockTransfer();

            List<StockTransferItem> TData = new List<StockTransferItem>();
            StockTransferItem tda = new StockTransferItem();

            ic.Flocationlst = BindFlocation();
            ic.Tlocationlst = BindTlocation();
            ic.FBinlst = BindFBin();
            ic.TBinlst = BindTBin();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new StockTransferItem();
                    tda.Itemlst = BindItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else

            {
            }
            ic.StockTransferItemLst = TData;
            return View(ic);
        }
        public IActionResult ListStockTransfer()
        {
            return View();
        }
        public JsonResult GetItemGrpJSON()
        {
            StockTransferItem model = new StockTransferItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = StockTransferService.Item;
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
                string var = "";
                string uom = "";
                string bin = "";
                string rate = "";
                dt = StockTransferService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    var = dt.Rows[0]["VARIANT"].ToString();
                    uom = dt.Rows[0]["UOM"].ToString();
                    bin = dt.Rows[0]["BIN_NO"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();


                }

                var result = new { var = var, uom = uom, bin = bin, rate = rate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> Binditem()
        {
            try
            {
                DataTable dtDesg = StockTransferService.Item;
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
        public List<SelectListItem> BindFlocation()
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetFlocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCATION_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTlocation()
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetTlocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCATION_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindFBin()
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetFBin();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTBin()
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetTBin();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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