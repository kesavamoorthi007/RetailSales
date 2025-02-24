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

            ic.InvoiceNo = "RETAIL/INV-1 /24-25";
            ic.InvoiceDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("PurchaseOrder");
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
                    tda.Varientlst = BindVarient("");
                    tda.UOMlst = BindUOM();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                

            }
            ic.SalesInvoiceLst = TData;
            return View(ic);
        }
        //public ActionResult Purchaseorder(Purchaseorder cy, string id)
        //{

        //    try
        //    {
        //        cy.ID = id;
        //        string Strout = SalesInvoiceService.SalesInvoiceCRUD(cy);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (cy.ID == null)
        //            {
        //                TempData["notice"] = "PurchaseOrder Inserted Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = "PurchaseOrder Updated Successfully...!";
        //            }
        //            return RedirectToAction("ListPurchaseorder");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit Purchaseorder";
        //            TempData["notice"] = Strout;
        //            //return View();
        //        }

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(cy);
        //}
         
        public ActionResult GetVarientDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                //string des = "";
                string uom = "";
                string hsn = "";
                string rate = "";
               
                dt = SalesInvoiceService.GetVarientDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    //des = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
                    hsn = dt.Rows[0]["HSCODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    
                }

                var result = new { uom = uom, hsn = hsn, rate = rate };
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



                dt = datatrans.GetData("SELECT CONVRT_FACTOR FROM UOM_CONVERT WHERE SRC_UOM = '" + uom  + "' AND DEST_UOM ='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {
                    cf = dt.Rows[0]["CONVRT_FACTOR"].ToString();

                }

                var result = new { cf = cf };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListSalesInvoice()
        {
            return View();
        }
        public JsonResult GetCityJSON(string cityid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindCity(cityid));
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
       
        public ActionResult MyListSalesInvoicegrid(string strStatus)
        {
            List<SalesInvoicegrid> Reg = new List<SalesInvoicegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalesInvoiceService.GetAllSalesInvoice(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string GoToSales = string.Empty;
                string report = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                   
                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Generated")
                    {
                        GoToSales = "<img src='../Images/tick.png' alt='Moved to Quote' width='20' />";
                        EditRow = "";
                        report = "<a href=Invoice?id=" + dtUsers.Rows[i]["ID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";

                    }
                    else
                    {
                        GoToSales = "<a href=ViewSalesReturn?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/back.png' alt='View Details' width='20' /></a>";
                        EditRow = "<a><img src='../Images/edit.png' alt='Edit' /></a>";
                        report = "<a href=Invoice?id=" + dtUsers.Rows[i]["ID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";


                    }
                    DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";

                }
                else
                {
                    DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                }
                Reg.Add(new SalesInvoicegrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    invno = dtUsers.Rows[i]["INVOICE_NO"].ToString(),
                    invdate = dtUsers.Rows[i]["INV_DATE"].ToString(),
                    customer = dtUsers.Rows[i]["CUSTOMER"].ToString(),
                    address = dtUsers.Rows[i]["ADDRESS"].ToString(),
                    totalamount = dtUsers.Rows[i]["TOTAL_AMOUNT"].ToString(),
                    editrow = EditRow,
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
                ic.Remarks = dt.Rows[0]["REMARKS"].ToString();
                ic.TotDis = dt.Rows[0]["DISCOUNT"].ToString();
                ic.Total = dt.Rows[0]["TOTAL_AMOUNT"].ToString();
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
                    tda.Item = dtt.Rows[i]["ITEM"].ToString();
                    tda.Description = dtt.Rows[i]["VARIENT"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Bin = dtt.Rows[i]["BIN_NO"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
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
       
    }
}
