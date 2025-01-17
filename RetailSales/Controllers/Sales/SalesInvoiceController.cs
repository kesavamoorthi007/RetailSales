using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Sales
{
    public class SalesInvoiceController : Controller
    {
        ISalesInvoiceService SalesInvoiceService;
        public SalesInvoiceController(ISalesInvoiceService _SalesInvoiceService)
        {
            SalesInvoiceService = _SalesInvoiceService;
        }
        public IActionResult SalesInvoice(string id)
        {
            SalesInvoice ic = new SalesInvoice();

            List<SalesInvoiceItem> TData = new List<SalesInvoiceItem>();
            SalesInvoiceItem tda = new SalesInvoiceItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new SalesInvoiceItem();
                    //tda.ItemGrouplst = BindItemGrplst();
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
                string EditRow = string.Empty;
                string GoToSales = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                   
                    if (dtUsers.Rows[i]["STATUS"].ToString() == "Generated")
                    {
                        GoToSales = "<img src='../Images/tick.png' alt='Moved to Quote' width='20' />";
                        EditRow = "";
                    }
                    else
                    {
                        GoToSales = "<a href=ViewSalesReturn?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                        EditRow = "<a><img src='../Images/edit.png' alt='Edit' /></a>";


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
    }
}
