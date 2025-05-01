using Microsoft.AspNetCore.Mvc;
using RetailSales.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Services;
using static RetailSales.Models.StockTransferItem;
using RetailSales.Interface.Purchase;
using RetailSales.Services.Purchase;
using RetailSales.Services.Sales;
using RetailSales.Services.Inventory;

namespace RetailSales.Controllers
{
    public class StockTransferController : Controller
    {
        IStockTransferService StockTransferService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public StockTransferController(IStockTransferService _StockTransferService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            StockTransferService = _StockTransferService;
        }
        public IActionResult StockTransfer(string id)
        {
            StockTransfer ic = new StockTransfer();

         
            ic.DocumentDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ic.FLoclst = BindFLocation();
            ic.Flocation = "2006";
            ic.TLoclst = BindTLocation();
            ic.Tlocation = "1007";
            DataTable dtv = datatrans.GetSequence("Stock Transfer");

            if (dtv.Rows.Count > 0)
            {
                ic.Documentid = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }
            List<StockTransferItem> TData = new List<StockTransferItem>();
            StockTransferItem tda = new StockTransferItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new StockTransferItem();
                    tda.Itemlst = BindItem();
                    tda.Productlst = BindProduct("");
                    tda.Varientlst = BindVarient("");
                    tda.FBinlst = BindFBin();
                    tda.TBinlst = BindTBin();
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
        [HttpPost]
        public ActionResult StockTransfer(StockTransfer Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = StockTransferService.StockTransferCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "StockTransfer Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StockTransfer Updated Successfully...!";
                    }
                    return RedirectToAction("ListStockTransfer");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StockTransfer";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }
        public IActionResult ListStockTransfer()
        {
            return View();
        }
        private List<SelectListItem> BindTLocation()
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetTLocation();
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

        private List<SelectListItem> BindFLocation()
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetFLocation();
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
                DataTable dtDesg = StockTransferService.GetItem();
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
        public ActionResult GetVarientDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string uom = "";
                string rate = "";
                string stockqty = "";
                string frombin = "";

                dt = StockTransferService.GetVarientDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                }

                dt = StockTransferService.GetStockDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    stockqty = dt.Rows[0]["BALANCE_QTY"].ToString();
                }

                dt = StockTransferService.GetFBinDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    frombin = dt.Rows[0]["BIN_ID"].ToString();
                }

                var result = new { uom = uom, rate = rate, stockqty = stockqty, frombin = frombin };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult GetStockDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        string stock = "";
        //        string qty = "";
        //        string rate = "";
        //        dt = StockTransferService.GetVarientDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {

        //            uom = dt.Rows[0]["UOM"].ToString();
        //            qty = dt.Rows[0]["QTY"].ToString();
        //            rate = dt.Rows[0]["RATE"].ToString();

        //        }

        //        var result = new { uom = uom, qty = qty, rate = rate };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public JsonResult GetProductJSON(string ItemId)
        {
            return Json(BindProduct(ItemId));
        }

        public List<SelectListItem> BindProduct(string id)
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetProduct(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROD_NAME"].ToString(), Value = dtDesg.Rows[i]["PRO_NAME_BASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetVarientJSON(string id)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindVarient(id));
        }
        public List<SelectListItem> BindVarient(string id)
        {
            try
            {
                DataTable dtDesg = StockTransferService.GetVariant(id);
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


        //public List<SelectListItem> BindFlocation()
        //{
        //    try
        //    {
        //        DataTable dtDesg = StockTransferService.GetFlocation();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCATION_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<SelectListItem> BindTlocation()
        //{
        //    try
        //    {
        //        DataTable dtDesg = StockTransferService.GetTlocation();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCATION_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public JsonResult GetFBinGrpJSON()
        {
            StockTransferItem model = new StockTransferItem();
            model.FBinlst = BindFBin();
            return Json(BindFBin());
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
        public JsonResult GetTBinGrpJSON()
        {
            StockTransferItem model = new StockTransferItem();
            model.TBinlst = BindTBin();
            return Json(BindTBin());
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

        public ActionResult MyListStockTransfergrid(string strStatus)
        {
            List<StockTransfergrid> Reg = new List<StockTransfergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = StockTransferService.GetAllStockTransferGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string View = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewStockTransfer?id=" + dtUsers.Rows[i]["ST_BASIC_ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    //DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ST_BASIC_ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' width='20' /></a>";
                }
                else
                {
                    //View = "<a href=ViewStockTransfer?id=" + dtUsers.Rows[i]["ST_BASIC_ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    //DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ST_BASIC_ID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                }
                Reg.Add(new StockTransfergrid
                {
                    id = dtUsers.Rows[i]["ST_BASIC_ID"].ToString(),
                    documentid = dtUsers.Rows[i]["STOCK_TRANSFER_ID"].ToString(),
                    stocktransferdate = dtUsers.Rows[i]["STOCK_TRANSFER_DATE"].ToString(),
                    fromLoc = dtUsers.Rows[i]["FROM_LOCATION"].ToString(),
                    toLoc = dtUsers.Rows[i]["TO_LOCATION"].ToString(),
                    fBin = dtUsers.Rows[i]["FBIN"].ToString(),
                    tBin = dtUsers.Rows[i]["TOBIN"].ToString(),
                    view = View,
                    delrow = DeleteRow,

                });
            }
            return Json(new
            {
                Reg
            });
        }
        public IActionResult ViewStockTransfer(string id)
        {
            StockTransfer ic = new StockTransfer();

            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = StockTransferService.GetStockTransfer(id);
            if (dt.Rows.Count > 0)
            {
                ic.Documentid = dt.Rows[0]["STOCK_TRANSFER_ID"].ToString();
                ic.DocumentDate = dt.Rows[0]["STOCK_TRANSFER_DATE"].ToString();
                ic.Flocation = dt.Rows[0]["FROM_LOCATION"].ToString();
                ic.Tlocation = dt.Rows[0]["TO_LOCATION"].ToString();
                
            }

            List<StockTransferItem> TData = new List<StockTransferItem>();
            StockTransferItem tda = new StockTransferItem();




            dtt = StockTransferService.GetStockTransferItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {

                    tda = new StockTransferItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                    tda.Stock = dtt.Rows[i]["STOCK"].ToString();
                    tda.FBin = dtt.Rows[i]["FBIN"].ToString();
                    tda.TBin = dtt.Rows[i]["TOBIN"].ToString();
                    tda.Qty = dtt.Rows[i]["QUANTITY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();

                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.StockTransferItemLst = TData;
            return View(ic);
        }


        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = StockTransferService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListStockTransfer");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListStockTransfer");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = StockTransferService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListStockTransfer");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListStockTransfer");
            }
        }

    }


}
