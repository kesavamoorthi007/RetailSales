using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using System.Data;
using AspNetCore.Reporting;
using RetailSales.Services.Purchase;
using RetailSales.Services.Master;

namespace RetailSales.Controllers.Sales
{
    public class SalesInvoiceController : Controller
    {
        ISalesInvoiceService SalesInvoiceService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public SalesInvoiceController(ISalesInvoiceService _SalesInvoiceService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            SalesInvoiceService = _SalesInvoiceService;
            this._WebHostEnvironment = WebHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public IActionResult SalesInvoice(string id)
        {
            SalesInvoice ic = new SalesInvoice();
            ic.Statelst = BindState();
            ic.Citylst = BindCity("");

            //ic.InvoiceNo = "SI/INV-4 /24-25";
            ic.InvoiceDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("Sales");
            if (dtv.Rows.Count > 0)
            {
                ic.InvoiceNo = dtv.Rows[0]["PREFIX"].ToString() + "/" + dtv.Rows[0]["SUFFIX"].ToString() + "/" + dtv.Rows[0]["last"].ToString();
            }


            List<SalesInvoiceItem> TData = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new SalesInvoiceItem();
                    tda.Itemlst = BindItem();
                    tda.Productlst = BindProduct("");
                    tda.Varientlst = BindVarient("");
                    tda.UOMlst = BindUOM();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt = SalesInvoiceService.GetEditSalesInvoice(id);
                if (dt.Rows.Count > 0)
                {
                    ic.InvoiceNo = dt.Rows[0]["INVOICE_NO"].ToString();
                    ic.InvoiceDate = dt.Rows[0]["INV_DATE"].ToString();
                    ic.Customer = dt.Rows[0]["CUSTOMER"].ToString();
                    ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ic.Statelst = BindState();
                    ic.State = dt.Rows[0]["STATE"].ToString();
                    ic.Citylst = BindCity(ic.State);
                    ic.City = dt.Rows[0]["CITY"].ToString();                   
                    ic.Mobile = dt.Rows[0]["MOBILE"].ToString();
                    ic.Gross = dt.Rows[0]["GROSS"].ToString();
                    ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                    ic.Frieghtcharge = dt.Rows[0]["FRIGHT"].ToString();
                    ic.Round = dt.Rows[0]["ROUND_OFF"].ToString();
                    ic.Net = dt.Rows[0]["NET"].ToString();
                    ic.Amountinwords = dt.Rows[0]["AMTINWORDS"].ToString();
                    ic.Remarks = dt.Rows[0]["NARRATION"].ToString();                    
                    ic.ID = id;

                }
                DataTable dtt = new DataTable();
                dtt = SalesInvoiceService.GetEditSalesInvoiceItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new SalesInvoiceItem();
                        tda.Itemlst = BindItem();
                        tda.Item = dtt.Rows[i]["ITEM"].ToString();
                        tda.Productlst = BindProduct(tda.Item);
                        tda.Product = dtt.Rows[i]["PRODUCT"].ToString();
                        tda.Varientlst = BindVarient(tda.Product);
                        tda.Varient = dtt.Rows[i]["VARIENT"].ToString();
                        tda.Hsn = dtt.Rows[i]["HSN_CODE"].ToString();
                        tda.UOM = dtt.Rows[i]["UOM"].ToString();
                        tda.Qty = dtt.Rows[i]["QTY"].ToString();
                        tda.UOMlst = BindUOM();
                        tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                        tda.CF = dtt.Rows[i]["CF"].ToString();
                        tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                        tda.Rate = dtt.Rows[i]["RATE"].ToString();
                        tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.Discount = dtt.Rows[i]["DISCOUNT"].ToString();
                        tda.Total = dtt.Rows[i]["TOTAL"].ToString();
                        tda.ID = id;
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            ic.SalesInvoiceLst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult SalesInvoice(SalesInvoice cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = SalesInvoiceService.SalesInvoiceCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "SalesInvoice Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesInvoice Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalesInvoice");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesInvoice";
                    TempData["notice"] = Strout;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public IActionResult ViewSalesInvoice(string id)
        {
            SalesInvoice ic = new SalesInvoice();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = SalesInvoiceService.GetSalesInvoice(id);
            if (dt.Rows.Count > 0)
            {
                ic.InvoiceNo = dt.Rows[0]["INVOICE_NO"].ToString();
                ic.InvoiceDate = dt.Rows[0]["INV_DATE"].ToString();
                ic.Customer = dt.Rows[0]["CUSTOMER"].ToString();
                ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                ic.State = dt.Rows[0]["STATE_NAME"].ToString();
                ic.City = dt.Rows[0]["CITY_NAME"].ToString();
                ic.Mobile = dt.Rows[0]["MOBILE"].ToString();
                ic.Gross = dt.Rows[0]["GROSS"].ToString();
                ic.Net = dt.Rows[0]["NET"].ToString();
                ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                ic.Frieghtcharge = dt.Rows[0]["FRIGHT"].ToString();
                ic.Round = dt.Rows[0]["ROUND_OFF"].ToString();
                ic.Amountinwords = dt.Rows[0]["AMTINWORDS"].ToString();
                ic.Remarks = dt.Rows[0]["NARRATION"].ToString();
                ic.ID = id;

            }

            List<SalesInvoiceItem> Data = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();


            dtt = SalesInvoiceService.GetSalesInvoiceItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new SalesInvoiceItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN_CODE"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Bin = dtt.Rows[i]["BIN_NO"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CF"].ToString();
                    tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.DiscPer = dtt.Rows[i]["DISC_PER"].ToString();
                    tda.Discount = dtt.Rows[i]["DISCOUNT"].ToString();
                    tda.Total = dtt.Rows[i]["TOTAL"].ToString();
                    tda.ID = id;
                    Data.Add(tda);
                }
            }
            ic.SalesInvoiceLst = Data;
            return View(ic);
        }

        public IActionResult ViewSalesReturn(string id)
        {
            SalesInvoice ic = new SalesInvoice();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = SalesInvoiceService.GetSalesInvoice(id);
            if (dt.Rows.Count > 0)
            {
                ic.InvoiceNo = dt.Rows[0]["INVOICE_NO"].ToString();
                ic.InvoiceDate = dt.Rows[0]["INV_DATE"].ToString();
                ic.Customer = dt.Rows[0]["CUSTOMER"].ToString();
                ic.Address = dt.Rows[0]["ADDRESS"].ToString();
                ic.Mobile = dt.Rows[0]["MOBILE"].ToString();
                ic.Gross = dt.Rows[0]["GROSS"].ToString();
                ic.Net = dt.Rows[0]["NET"].ToString();
                ic.Disc = dt.Rows[0]["DISCOUNT"].ToString();
                ic.Frieghtcharge = dt.Rows[0]["FRIGHT"].ToString();
                ic.Round = dt.Rows[0]["ROUND_OFF"].ToString();
                ic.Amountinwords = dt.Rows[0]["AMTINWORDS"].ToString();
                ic.Remarks = dt.Rows[0]["NARRATION"].ToString();
                ic.ID = id;

            }

            List<SalesInvoiceItem> Data = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();


            dtt = SalesInvoiceService.GetSalesInvoiceItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new SalesInvoiceItem();
                    tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                    tda.Product = dtt.Rows[i]["PROD_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN_CODE"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Bin = dtt.Rows[i]["BIN_NO"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.DestUOM = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CF"].ToString();
                    tda.CfQty = dtt.Rows[i]["CF_QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.Discount = dtt.Rows[i]["DISCOUNT"].ToString();
                    tda.Total = dtt.Rows[i]["TOTAL"].ToString();
                    tda.ID = id;
                    Data.Add(tda);
                }
            }
            ic.SalesInvoiceLst = Data;
            return View(ic);
        }
        [HttpPost]
        public ActionResult ViewSalesReturn(SalesInvoice Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = SalesInvoiceService.InvoicetoReturn(Cy.ID);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SalesReturn Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesReturn Generated Successfully...!";
                    }
                    return RedirectToAction("ListSalesInvoice");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesInvoice";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListSalesEnquiry");
        }

        public IActionResult ListSalesInvoice()
        {
            return View();
        }

        public ActionResult MyListSalesInvoicegrid(string strStatus)
        {
            List<SalesInvoicegrid> Reg = new List<SalesInvoicegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalesInvoiceService.GetAllSalesInvoice(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                //string EditRow = string.Empty;
                string View = string.Empty;
                string GoToSales = string.Empty;
                string report = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewSalesInvoice?id=" + dtUsers.Rows[i]["SAL_INV_BASICID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";

                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Generated")
                    {
                        GoToSales = "<img src='../Images/tick.png' alt='Moved to Quote' width='20' />";
                        //EditRow = "";
                        report = "<a><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                    }
                    else
                    {
                        GoToSales = "<a href=ViewSalesReturn?id=" + dtUsers.Rows[i]["SAL_INV_BASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/back.png' alt='View Details' width='20' /></a>";
                        //EditRow = "<a href=SalesInvoice?id=" + dtUsers.Rows[i]["SAL_INV_BASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit 'width='20'  /></a>";
                        report = "<a><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";


                    }

                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["SAL_INV_BASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                    
                    //DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";

                }
                else
                {
                    GoToSales = "";
                    report = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["SAL_INV_BASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                    //DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                }
                Reg.Add(new SalesInvoicegrid
                {
                    id = dtUsers.Rows[i]["SAL_INV_BASICID"].ToString(),
                    invno = dtUsers.Rows[i]["INVOICE_NO"].ToString(),
                    invdate = dtUsers.Rows[i]["INV_DATE"].ToString(),
                    customer = dtUsers.Rows[i]["CUSTOMER"].ToString(),
                    address = dtUsers.Rows[i]["ADDRESS"].ToString(),
                    totalamount = dtUsers.Rows[i]["NET"].ToString(),
                    //editrow = EditRow,
                    view = View,
                    move = GoToSales,
                    delrow = DeleteRow,
                    report = report,

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult GetVarientDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                //string des = "";
                string uom = "";
                string hsn = "";
                string rate = "";
                string uomid = "";
                string stockqty = "";
               
                dt = SalesInvoiceService.GetVarientDetails(ItemId);
                
                if (dt.Rows.Count > 0)
                {
                    //des = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
                    uomid = dt.Rows[0]["UOM_ID"].ToString();
                    hsn = dt.Rows[0]["HSCODE"].ToString();
                    rate = dt.Rows[0]["SALES_RATE"].ToString();
                    if (rate == "")
                    {
                        rate = "0";
                    }

                }
                dt = SalesInvoiceService.GetStockDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    stockqty = dt.Rows[0]["BALANCE_QTY"].ToString();
                }
                else
                {
                    stockqty = "0";
                }

                var result = new { uomid = uomid, uom = uom, hsn = hsn, rate = rate, stockqty = stockqty };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetUOMDetail(string ItemId,string uom)
        {
            try
            {
                DataTable dt = new DataTable();

                //string des = "";
                string cf = "";



                dt = datatrans.GetData("SELECT CF FROM UOM_CONVERT WHERE SRC_UOM = '" + uom  + "' AND DEST_UOM ='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {
                    cf = dt.Rows[0]["CF"].ToString();

                }
                else 
                {
                    cf = "1";
                }

                var result = new { cf = cf };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public JsonResult GetCityJSON(string cityid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindCity(cityid));
        }

        public JsonResult GetProductJSON(string ItemId)
        {
            return Json(BindProduct(ItemId));
        }

        public JsonResult GetVarientJSON(string id)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindVarient(id));
        }

        public JsonResult GetItemGrpJSON()
        {
            SalesInvoiceItem model = new SalesInvoiceItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }

        public JsonResult GetUOMGrpJSON()
        {
            PurchaseorderItem model = new PurchaseorderItem();
            model.DUOMlst = BindUOM();
            return Json(BindUOM());
        }
        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetUOM();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["UOM_CODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCity(string cityid)
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetCity(cityid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetItem();
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
                DataTable dtDesg = SalesInvoiceService.GetProduct(id);
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

        public List<SelectListItem> BindVarient(string id)
        {
            try
            {
                DataTable dtDesg = SalesInvoiceService.GetVariant(id);
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
       
        
        
        public async Task<IActionResult> Invoice(string id)
        {

            string mimtype = "";
            int extension = 1;

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\SalesInv.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var basic = await SalesInvoiceService.GetBasicItem(id);
            // var Detail = await SalesInvoiceService.GetExinvItemDetail(id);
            //var terms = await ProFormaInvoiceService.GetPinvtermsDetail(id);
            LocalReport localReport = new LocalReport(path);

            localReport.AddDataSource("salesinv", basic);

            //localReport.AddDataSource("InvDet", Detail);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = SalesInvoiceService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesInvoice");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesInvoice");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = SalesInvoiceService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesInvoice");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesInvoice");
            }
        }

    }
}
