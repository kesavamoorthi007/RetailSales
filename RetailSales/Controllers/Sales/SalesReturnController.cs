using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using RetailSales.Services;
using System.Data;

namespace RetailSales.Controllers.Sales
{
    public class SalesReturnController : Controller
    {
        ISalesReturnService SalesReturnService;
        public SalesReturnController(ISalesReturnService _SalesReturnService)
        {
            SalesReturnService = _SalesReturnService;
        }
        public IActionResult ListSalesReturn()
        {
            return View();
        }
        public ActionResult MyListSalesReturngrid(string strStatus)
        {
            List<SalesReturngrid> Reg = new List<SalesReturngrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SalesReturnService.GetAllSalesReturn(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;
                string GoTo = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    Edit = "<a><img src='../Images/edit.png' alt='Edit' width='20' /></a>";
                    GoTo = "<a href=/CreditNote/SalesCreditNote?id=" + dtUsers.Rows[i]["RETURN_TYPE"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/sharing.png' alt='Deactivate' width='20' /></a>";
                    Delete = "<a><img src='../Images/Inactive.png' alt='Deactivate' width='20' /></a>";
                    
                }
                else
                {
                    Edit = "";
                    GoTo = "";
                    Delete = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                }
                Reg.Add(new SalesReturngrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    invno = dtUsers.Rows[i]["INVOICE_NO"].ToString(),
                    invdate = dtUsers.Rows[i]["INV_DATE"].ToString(),
                    customer = dtUsers.Rows[i]["CUSTOMER"].ToString(),
                    doc = dtUsers.Rows[i]["DOC_NO"].ToString(),
                    date = dtUsers.Rows[i]["DOC_DATE"].ToString(),
                    type = dtUsers.Rows[i]["RETURN_TYPE"].ToString(),
                    edit = Edit,
                    go = GoTo,
                    delete = Delete,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
