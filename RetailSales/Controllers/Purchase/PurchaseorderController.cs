using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
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
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public PurchaseorderController(IPurchaseorderService _PurchaseorderService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            PurchaseorderService = _PurchaseorderService;
        }
        public IActionResult Purchaseorder(string id)
        {
            Purchaseorder ic = new Purchaseorder();

            ic.Suplst= BindSupplier();

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.Itemlst = BindItem();
                    tda.Varientlst = BindVarient("");
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
        [HttpPost]
        public ActionResult Purchaseorder(Purchaseorder cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = PurchaseorderService.PurchaseorderCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "PurchaseOrder Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseOrder Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseorder");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Purchaseorder";
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
        public IActionResult ViewPurchaseOrder(string id)
        {
            Purchaseorder ic = new Purchaseorder();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = PurchaseorderService.GetPurchasOrder(id);
            if (dt.Rows.Count > 0)
            {
                ic.po = dt.Rows[0]["PONO"].ToString();
                ic.Podate = dt.Rows[0]["PODATE"].ToString();
                ic.Suppid = dt.Rows[0]["SUP_NAME"].ToString();
                ic.Supplieraddress = dt.Rows[0]["ADDRESS"].ToString();
                ic.Country = dt.Rows[0]["COUNTRY"].ToString();
                ic.State = dt.Rows[0]["STATE"].ToString();
                ic.City = dt.Rows[0]["CITY"].ToString();
                ic.refno = dt.Rows[0]["REF_NO"].ToString();
                ic.refdate = dt.Rows[0]["REF_DATE"].ToString();
                ic.Total = dt.Rows[0]["AMOUNT"].ToString();
                ic.Amountinwords = dt.Rows[0]["AMTINWORDS"].ToString();
                ic.Narration = dt.Rows[0]["NARRATION"].ToString();
                ic.ID = id;

            }

            List<PurchaseorderItem> TData = new List<PurchaseorderItem>();
            PurchaseorderItem tda = new PurchaseorderItem();



            dtt = PurchaseorderService.GetPurchasOrderItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new PurchaseorderItem();
                    tda.Item = dtt.Rows[i]["ITEM"].ToString();
                    tda.Varient = dtt.Rows[i]["VARIANT"].ToString();
                    tda.UOM = dtt.Rows[i]["UOM"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSN"].ToString();
                    tda.Qty = dtt.Rows[i]["QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.PurchaseorderLst = TData;
            return View(ic);
        }
        public IActionResult ListPurchaseorder()
        {
            return View();
        }
        public ActionResult GetVarientDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string des = "";
                string uom = "";
                string hsn = "";
                string rate = "";
                dt = PurchaseorderService.GetVarientDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    des = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();
                    uom = dt.Rows[0]["UOM_CODE"].ToString();
                    hsn = dt.Rows[0]["HSN_CODE"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();


                }

                var result = new { des = des, uom = uom, hsn = hsn, rate = rate };
                return Json(result);
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
        public JsonResult GetItemGrpJSON()
        {
            PurchaseorderItem model = new PurchaseorderItem();
            model.Itemlst = BindItem();
            return Json(BindItem());
        }
       
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = PurchaseorderService.GetItem();
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
                DataTable dtDesg = PurchaseorderService.GetVariant(id);
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
        public ActionResult GetSupplierDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string add = "";
                string state = "";
                string city = "";
                dt = PurchaseorderService.GetSupplierDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add = dt.Rows[0]["ADDRESS"].ToString();
                    state = dt.Rows[0]["STATE_NAME"].ToString();
                    city = dt.Rows[0]["CITY_NAME"].ToString();
                 


                }

                var result = new { add = add, state = state, city = city };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    EditRow = "<a href=Purchaseorder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }

                Reg.Add(new ListPurchaseordergrid
                {
                    id = dtUsers.Rows[i]["POBASICID"].ToString(),
                    po = dtUsers.Rows[i]["PONO"].ToString(),
                    podate = dtUsers.Rows[i]["PODATE"].ToString(),
                    sup = dtUsers.Rows[i]["SUP_NAME"].ToString(),
                    refno = dtUsers.Rows[i]["REF_NO"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = PurchaseorderService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPurchaseorder");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPurchaseorder");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = PurchaseorderService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPurchaseorder");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPurchaseorder");
            }
        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("select ID,SUPPLIER_NAME from SUPPLIER where IS_ACTIVE='Y'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SUPPLIER_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
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