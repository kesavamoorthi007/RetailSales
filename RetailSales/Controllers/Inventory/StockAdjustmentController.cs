using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Inventory;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using RetailSales.Models.Inventory;
using RetailSales.Services.Master;
using RetailSales.Services.Purchase;
using RetailSales.Services.Sales;

namespace RetailSales.Controllers.Inventory
{
    public class StockAdjustmentController : Controller
    {
        IStockAdjustmentService StockAdjustmentService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public StockAdjustmentController(IStockAdjustmentService _StockAdjustmentService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            StockAdjustmentService = _StockAdjustmentService;
        }
        public IActionResult StockAdjustment(string id)
        {
            StockAdjustment ic = new StockAdjustment();
            ic.Locationlst = BindLocation();
            ic.Type = "Addition";
            ic.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Stock Adjustment");
            if (dtv.Rows.Count > 0)
            {
                ic.DocId = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }

            List<StockAdjustmentItem> TData = new List<StockAdjustmentItem>();
            StockAdjustmentItem tda = new StockAdjustmentItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new StockAdjustmentItem();
                    tda.Itemlst = BindItem();
                    tda.Productlst = BindProduct("");
                    tda.Variantlst = BindVariant("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt = StockAdjustmentService.GetEditStockAdjustment(id);
                if (dt.Rows.Count > 0)
                {
                    ic.Locationlst = BindLocation();
                    ic.Location = dt.Rows[0]["LOCATION"].ToString();
                    ic.Type = dt.Rows[0]["TYPE"].ToString();
                    ic.DocId = dt.Rows[0]["DOCID"].ToString();
                    ic.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ic.Reason = dt.Rows[0]["REASON"].ToString();
                    ic.ID = id;

                }
                DataTable dtt = new DataTable();
                dtt = StockAdjustmentService.GetEditStockAdjustmentItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new StockAdjustmentItem();
                        tda.Itemlst = BindItem();
                        tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                        tda.Productlst = BindProduct(tda.Item);
                        tda.Product = dtt.Rows[i]["PRODUCT"].ToString();
                        tda.Variantlst = BindVariant(tda.Product);
                        tda.Variant = dtt.Rows[i]["VARIANT"].ToString();
                        tda.Unit = dtt.Rows[i]["UOM"].ToString();
                        tda.StockQty = dtt.Rows[i]["STOCKQTY"].ToString();
                        tda.Qty = dtt.Rows[i]["QTY"].ToString();
                        tda.Rate = dtt.Rows[i]["RATE"].ToString();
                        tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.Isvalid = "Y";
                        tda.ID = id;
                        TData.Add(tda);
                    }
                }
            }
            ic.StockAdjustmentList = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult StockAdjustment(StockAdjustment cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = StockAdjustmentService.StockAdjustmentCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "StockAdjustment Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StockAdjustment Updated Successfully...!";
                    }
                    return RedirectToAction("ListStockAdjustment");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StockAdjustment";
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


        public IActionResult ListStockAdjustment()
        {
            return View();
        }

        public ActionResult MyListStockAdjustmentgrid(string strStatus)
        {
            List<ListStockAdjustmentgrid> Reg = new List<ListStockAdjustmentgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = StockAdjustmentService.GetAllStockAdjustment(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                //string Edit = string.Empty;
                //string Delete = string.Empty;
                string View = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewStockAdjustment?id=" + dtUsers.Rows[i]["STKADJBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    //Edit = "<a href=StockAdjustment?id=" + dtUsers.Rows[i]["STKADJBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    //Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["STKADJBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    //Edit = "";
                    //Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["STKADJBASICID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }

                Reg.Add(new ListStockAdjustmentgrid
                {
                    id = dtUsers.Rows[i]["STKADJBASICID"].ToString(),
                    location = dtUsers.Rows[i]["LOCATION_NAME"].ToString(),
                    type = dtUsers.Rows[i]["TYPE"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    view = View,
                    //edit = Edit,
                    //delete = Delete,

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public IActionResult ViewStockAdjustment(string id)
        {
            StockAdjustment ic = new StockAdjustment();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = StockAdjustmentService.GetStockAdjustment(id);
            if(dt.Rows.Count > 0)
            {
                ic.Location = dt.Rows[0]["LOCATION_NAME"].ToString();
                ic.Type = dt.Rows[0]["TYPE"].ToString();
                ic.DocId = dt.Rows[0]["DOCID"].ToString();
                ic.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ic.Reason = dt.Rows[0]["REASON"].ToString();
            }

            List<StockAdjustmentItem> TData = new List<StockAdjustmentItem>();
            StockAdjustmentItem tda = new StockAdjustmentItem();

            dtt = StockAdjustmentService.GetStockAdjustmentItem(id);
            if(dtt.Rows.Count > 0)
            {
                for(int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new StockAdjustmentItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Variant = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Unit = dtt.Rows[i]["UOM"].ToString();
                    tda.StockQty = dtt.Rows[i]["STOCKQTY"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();

                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.StockAdjustmentList = TData;
            return View(ic);
        }

        private List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = StockAdjustmentService.GetLocation();
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
            StockAdjustmentItem model = new StockAdjustmentItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }

        public JsonResult GetProductJSON(string ItemId)
        {
            return Json(BindProduct(ItemId));
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

        public List<SelectListItem> BindProduct(string id)
        {
            try
            {
                DataTable dtDesg = StockAdjustmentService.GetProduct(id);
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

        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string unit = "";
                string rate = "";
                string stockqty = "";

                dt = StockAdjustmentService.GetVariantDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UOM_CODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();


                }
                dt = StockAdjustmentService.GetStockDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    stockqty = dt.Rows[0]["BALANCE_QTY"].ToString();
                }
                var result = new { unit = unit, rate = rate, stockqty = stockqty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = StockAdjustmentService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListStockAdjustment");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListStockAdjustment");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = StockAdjustmentService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListStockAdjustment");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListStockAdjustment");
            }
        }

    }
}
