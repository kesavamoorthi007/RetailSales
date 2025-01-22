using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Purchase;
using RetailSales.Models;

using RetailSales.Services.Purchase;
using RetailSales.Services.Sales;
using System.Data;

namespace RetailSales.Controllers.Purchase
{
    public class PurchaseorderController : Controller
    {
        IPurchaseorderService PurchaseorderService;

        public PurchaseorderController(IPurchaseorderService _PurchaseorderService)
        {
            PurchaseorderService = _PurchaseorderService;
        }
        public IActionResult Purchaseorder(string id)
        {
            Purchaseorder ic = new Purchaseorder();

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PurchaseorderItem();
                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }


            }
            else
            {


            }
            ic.PurchaseorderLst = TData;
            return View(ic);
        }
        public IActionResult ListPurchaseorder()
        {
            return View();
        }
        public ActionResult MyListPurchaseordergrid(string strStatus)
        {
            List<ListPurchaseordergrid> Reg = new List<ListPurchaseordergrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = PurchaseorderService.GetAllListPurchaseorder(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                }
                else
                {
                    EditRow = "<a><img src='../Images/edit.png' alt='Edit' width='20' /></a>";
                    DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";

                }
                Reg.Add(new ListPurchaseordergrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    doc = dtUsers.Rows[i]["DOC_NO"].ToString(),
                    docdate = dtUsers.Rows[i]["DOC_DATE"].ToString(),
                    company = dtUsers.Rows[i]["COMPANY_NAME"].ToString(),
                    suppier = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}